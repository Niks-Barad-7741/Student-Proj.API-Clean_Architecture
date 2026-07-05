using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StudentProj.Application.DTOs
{
    public class SubjectDTO
    {
        public int Id { get; set; }
        public string SubjectName { get; set; }
        public int SubjectCode { get; set; }
        public int CourseId { get; set; }
    }

    public class CreateSubjectDTO
    {
        [Required]
        [StringLength(50)]
        public string SubjectName { get; set; }

        [Required]
        public int SubjectCode { get; set; }

        [Required]
        public int CourseId { get; set; }
    }
    public class UpdateSubjectDTO
    {
        [Required]
        [StringLength(50)]
        public string SubjectName { get; set; }

        [Required]
        public int SubjectCode { get; set; }

        [Required]
        public int CourseId { get; set; }
    }
}
