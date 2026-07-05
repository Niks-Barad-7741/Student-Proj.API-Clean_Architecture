using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StudentProj.Application.DTOs
{
    public class MenuDTO
    {
        public int Id { get; set; }
        public string MenuName { get; set; }
        public string MenuRoute { get; set; }
    }

    public class CreateMenuDTO
    {
        [Required]
        [StringLength(50)]
        public string MenuName { get; set; }

        [Required]
        [StringLength(100)]
        public string MenuRoute { get; set; }
    }
}
