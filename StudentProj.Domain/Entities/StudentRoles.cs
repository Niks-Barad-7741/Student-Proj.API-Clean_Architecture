//using Newtonsoft.Json;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data;
using System.Text.Json.Serialization;
namespace StudentProj.Domain.Entities
{
    public class StudentRoles
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Required]
        public int StudentId { get; set; }

        [Required]
        public int RoleId { get; set; }

        public bool IsDeleted { get; set; } = false;

        public DateTime? DeletedAt { get; set; }

        [ForeignKey("StudentId")]
        [JsonIgnore]
        public Student Student { get; set; }

        [ForeignKey("RoleId")]
        public Roles Role { get; set; }

    }
}
