using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StudentProj.Application.DTOs
{
    public class PermissionDTO
    {
        public int Id { get; set; }
        public string PermissionName { get; set; }
    }

    public class CreatePermissionDTO
    {
        [Required]
        [StringLength(50)]
        public string PermissionName { get; set; }
    }
}
