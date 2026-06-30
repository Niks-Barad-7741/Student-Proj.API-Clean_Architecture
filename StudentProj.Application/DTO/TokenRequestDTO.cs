using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StudentProj.Application.DTO
{
    public class TokenRequestDTO
    {
        [Required]
        public string AccessToken { get; set; }

        [Required]
        public string RefereshToken { get; set; }
    }
}