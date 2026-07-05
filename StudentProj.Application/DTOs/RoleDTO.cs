using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StudentProj.Application.DTOs
{
    public class RoleDTO
    {
        public int Id { get; set; }
        public string RoleName { get; set; }
    }

    public class CreateRoleDTO
    {
        [Required]
        [StringLength(50)]
        public string RoleName { get; set; }
    }
}
