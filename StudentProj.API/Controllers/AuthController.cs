using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Distributed;
using StudentProj.Application.DTO;
using StudentProj.Application.Interfaces;
using StudentProj.Core.Common;
using StudentProj.Core.Entities;
using StudentProj.Core.Enums;
using StudentProj.Core.Interface;
using StudentProj.DTO;
using System.Net;
using System.Net.Mail;
using System.Security.Claims;


namespace StudentProj.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IRegisterRepository _auth;
        private readonly ILoginRepository _login;
        private readonly ILoginService _loginService;
        private readonly IJwtService _jwtService;
        private readonly ILoggingService _loggingService;
        private readonly IStudent _studentRepo;
        private readonly IConfiguration _config;
        private readonly IDistributedCache _cache;

        public AuthController(
            IRegisterRepository auth,
            ILoginRepository login,
            ILoginService loginService,
            IJwtService jwtService,
            ILoggingService loggingService,
            IStudent studentRepo,
            IConfiguration config,
            IDistributedCache cache)
        {
            _auth = auth;
            _login = login;
            _loginService = loginService;
            _jwtService = jwtService;
            _loggingService = loggingService;
            _studentRepo = studentRepo;
            _config = config;
            _cache = cache;
        }

        [HttpPost("register")]
        public async Task<ActionResult> Register(RegisterDTO dto)
        {
            var existing = await _auth.GetStudentbyphoneasync(dto.Phone);
            if (existing != null)
            {
                await _loggingService.LogActivityAsync(dto.Name, dto.Email, "Registration Failed: Phone number already registered", HttpContext);
                var errorResponse = ApiResponse<object>.Create(ResponseStatus.UserAlreadyExist, "Phone number already registered!");
                return StatusCode(errorResponse.StatusCodes, errorResponse);
            }

            var student = new Student
            {
                Name = dto.Name,
                Email = dto.Email,
                Address = dto.Address,
                Phone = dto.Phone,
                PasswordHash = BCrypt.Net.BCrypt.HashPassword(dto.Password),
                CreatedAt = DateTimeHelper.GetIndianStandardTime(),
                CreatedBy = "User",
                IpAddress = IpHelper.GetClientIpAddress(HttpContext)
            };

            await _auth.RegisterAsync(student);

            var studentRole = await _auth.GetRoleByIdAsync(3);
            if (studentRole != null)
                await _auth.AssignRoleAsync(student.Id, studentRole.Id);

            var roles = await _auth.GetStudentRolesAsync(student.Id);
            var token = _jwtService.GenerateToken(student, roles);
            var refreshToken = _jwtService.GenerateRefreshToken();
            var permissions = await _loginService.GetStudentPermissionAsync(student.Id);

            student.RefereshToken = refreshToken;
            student.RefereshTokenExpiryTime = DateTimeHelper.GetIndianStandardTime().AddDays(7);
            
            await _studentRepo.UpdateStudentasync(student.Id, student);

            await _loggingService.LogActivityAsync(student.Name, student.Email, "Registration Succeeded", HttpContext);

            var authData = new LoginResponseDTO
            {
                Token = token,
                RefreshToken = refreshToken,
                Permissions = permissions
            };
            var response = ApiResponse<LoginResponseDTO>.Create(ResponseStatus.UserRegisterSuccessfully, authData);
            return StatusCode(response.StatusCodes, response);
        }

        [HttpPost("login")]
        public async Task<ActionResult> Login(LoginDTO dto)
        {
            var student = await _login.GetStudentbyemailasync(dto.Email);
            if (student == null)
            {
                await _loggingService.LogActivityAsync("Anonymous", dto.Email, "Login Failed: Invalid Email", HttpContext);
                var errorResponse = ApiResponse<object>.Create(ResponseStatus.InvalidCredentials, "Invalid email.");
                return StatusCode(errorResponse.StatusCodes, errorResponse);
            }

            bool isValid = BCrypt.Net.BCrypt.Verify(dto.Password, student.PasswordHash);
            if (!isValid)
            {
                await _loggingService.LogActivityAsync("Anonymous", dto.Email, "Login Failed: Invalid Password", HttpContext);
                var errorResponse = ApiResponse<object>.Create(ResponseStatus.InvalidCredentials, "Invalid Password.");
                return StatusCode(errorResponse.StatusCodes, errorResponse);
            }

            var roles = await _login.GetStudentRolesAsync(student.Id);
            var token = _jwtService.GenerateToken(student, roles);
            var refreshToken = _jwtService.GenerateRefreshToken();
            var permission = await _loginService.GetStudentPermissionAsync(student.Id);

            student.RefereshToken = refreshToken;
            student.RefereshTokenExpiryTime = DateTimeHelper.GetIndianStandardTime().AddDays(7);
            await _studentRepo.UpdateStudentasync(student.Id, student);

            await _loggingService.LogActivityAsync(student.Name, student.Email, "Login Succeeded", HttpContext);
            var authData = new LoginResponseDTO
            {
                Token = token,
                RefreshToken = refreshToken,
                Permissions = permission
            };
            var response = ApiResponse<LoginResponseDTO>.Create(ResponseStatus.UserLoginSuccessfully, authData);
            return StatusCode(response.StatusCodes, response);
        }

        [HttpPost("forgot-password")]
        public async Task<ActionResult> ForgotPassword([FromBody] ForgotPasswordDTO dto)
        {
            var ipaddress = HttpContext.Connection.RemoteIpAddress?.ToString() ?? "user";
            
            // Layer 1: IP Rate Limit (3 per 10 mins)
            string ipKey = $"otp_limit:ip:{ipaddress}";
            if (!await IsRateLimitAllowedAsync(ipKey, 3))
            {
                var errorResponse = ApiResponse<object>.Create(ResponseStatus.BadRequest, "Too Many OTP Requests from this IP. Try again after 10 minutes");
                return StatusCode(429, errorResponse);
            }

            // Layer 2: Email Rate Limit (5 per 1 hour)
            string emailKey = $"otp_limit:email:{dto.Email.Trim().ToLower()}";
            if (!await IsRateLimitAllowedAsync(emailKey, 5))
            {
                var errorResponse = ApiResponse<object>.Create(ResponseStatus.BadRequest, "Too Many OTP from This Email, Please Try again in After 1 Hour");
                return StatusCode(429, errorResponse);
            }

            var student = await _login.GetStudentbyemailasync(dto.Email);
            if (student == null)
            {
                var errorResponse = ApiResponse<object>.Create(ResponseStatus.UserNotFound, "User not found with that email.");
                return StatusCode(errorResponse.StatusCodes, errorResponse);
            }

            // Layer 3: DB Cooldown Block (1 minute)
            if (student.ResetOtpExpiry != null && DateTimeHelper.GetIndianStandardTime() < student.ResetOtpExpiry)
            {
                var errorResponse = ApiResponse<object>.Create(ResponseStatus.BadRequest, "An active OTP was already sent. Please wait 1 minute before requesting a new one.");
                return StatusCode(400, errorResponse);
            }

            // All checks passed! Increment counters
            await IncrementLimitCounterAsync(ipKey, TimeSpan.FromMinutes(10));
            await IncrementLimitCounterAsync(emailKey, TimeSpan.FromHours(1));

            var otp = new Random().Next(100000, 999999).ToString();
            
            student.ResetOtp = otp;
            student.ResetOtpExpiry = DateTimeHelper.GetIndianStandardTime().AddMinutes(1);
            await _studentRepo.UpdateStudentasync(student.Id, student);

            try
            {
                var emailSettings = _config.GetSection("EmailConfiguration");
                using var client = new SmtpClient(emailSettings["SmtpServer"], int.Parse(emailSettings["SmtpPort"]))
                {
                    UseDefaultCredentials = false,
                    Credentials = new NetworkCredential(emailSettings["SenderEmail"], emailSettings["Password"]),
                    EnableSsl = true
                };

                var mailMessage = new MailMessage
                {
                    From = new MailAddress(emailSettings["SenderEmail"], emailSettings["SenderName"]),
                    Subject = "Your Password Reset OTP",
                    Body = $"Your 6-digit OTP for password reset is: <b>{otp}</b><br/><br/>This code expires in 1 minute.",
                    IsBodyHtml = true
                };
                mailMessage.To.Add(dto.Email);
                await client.SendMailAsync(mailMessage);
            }
            catch (Exception ex)
            {
                var errorResponse = ApiResponse<object>.Create(ResponseStatus.BadRequest, $"Failed to send email. Error: {ex.Message}");
                return StatusCode(errorResponse.StatusCodes, errorResponse);
            }

            var response = ApiResponse<object>.Create(ResponseStatus.UserUpdatedSuccessfully, "OTP has been sent to your email.");
            return StatusCode(response.StatusCodes, response);
        }

        [HttpPost("reset-password")]
        public async Task<ActionResult> ResetPassword([FromBody] ResetPasswordDTO dto)
        {
            var student = await _login.GetStudentbyemailasync(dto.Email);
            if (student == null)
            {
                var errorResponse = ApiResponse<object>.Create(ResponseStatus.UserNotFound, "User not found with that email.");
                return StatusCode(errorResponse.StatusCodes, errorResponse);
            }

            if (student.ResetOtp != dto.Otp || student.ResetOtpExpiry == null || DateTimeHelper.GetIndianStandardTime() > student.ResetOtpExpiry)
            {
                var errorResponse = ApiResponse<object>.Create(ResponseStatus.BadRequest, "Invalid or Expired OTP.");
                return StatusCode(errorResponse.StatusCodes, errorResponse);
            }

            student.PasswordHash = BCrypt.Net.BCrypt.HashPassword(dto.NewPassword);
            student.ResetOtp = null; 
            student.ResetOtpExpiry = null;
            
            await _studentRepo.UpdateStudentasync(student.Id, student);

            var response = ApiResponse<object>.Create(ResponseStatus.UserUpdatedSuccessfully, "Password reset successfully. You can now login.");
            return StatusCode(response.StatusCodes, response);
        }

        [HttpPost("refresh")]
        public async Task<ActionResult> Refresh([FromBody] TokenRequestDTO dto)
        {
            if (string.IsNullOrEmpty(dto.AccessToken) || string.IsNullOrEmpty(dto.RefereshToken))
            {
                var error = ApiResponse<object>.Create(ResponseStatus.BadRequest, "Invalid client request.");
                return StatusCode(error.StatusCodes, error);
            }

            ClaimsPrincipal? principal = _jwtService.GetClaimsPrincipalFromExpiredToken(dto.AccessToken);
            if (principal == null)
            {
                var error = ApiResponse<object>.Create(ResponseStatus.BadRequest, "Invalid access token.");
                return StatusCode(error.StatusCodes, error);
            }

            var emailClaim = principal.FindFirst("Email") ?? principal.FindFirst(ClaimTypes.Email);
            var student = await _login.GetStudentbyemailasync(emailClaim.Value);
            if (student == null || student.RefereshToken != dto.RefereshToken || student.RefereshTokenExpiryTime <= DateTimeHelper.GetIndianStandardTime())
            {
                var error = ApiResponse<object>.Create(ResponseStatus.BadRequest, "Invalid refresh token or token has expired.");
                return StatusCode(error.StatusCodes, error);
            }

            var roles = await _login.GetStudentRolesAsync(student.Id);
            var newAccessToken = _jwtService.GenerateToken(student, roles);
            var newRefreshToken = _jwtService.GenerateRefreshToken();

            student.RefereshToken = newRefreshToken;
            student.RefereshTokenExpiryTime = DateTimeHelper.GetIndianStandardTime().AddDays(7);
            await _studentRepo.UpdateStudentasync(student.Id, student);

            var responseData = new LoginResponseDTO
            {
                Token = newAccessToken,
                RefreshToken = newRefreshToken
            };

            var success = ApiResponse<LoginResponseDTO>.Create(ResponseStatus.UserLoginSuccessfully, responseData);
            return StatusCode(success.StatusCodes, success);
        }

        private async Task<bool> IsRateLimitAllowedAsync(string key, int maxRequests)
        {
            var currentStr = await _cache.GetStringAsync(key);
            if (currentStr != null && int.TryParse(currentStr, out int current))
            {
                if (current >= maxRequests) return false;
            }
            return true;
        }

        private async Task IncrementLimitCounterAsync(string key, TimeSpan expiry)
        {
            var currentStr = await _cache.GetStringAsync(key);
            int current = 0;
            if (currentStr != null && int.TryParse(currentStr, out int val))
                current = val;
            current++;

            var cacheOptions = new DistributedCacheEntryOptions
            {
                AbsoluteExpirationRelativeToNow = expiry
            };
            await _cache.SetStringAsync(key, current.ToString(), cacheOptions);
        }
    }
}
