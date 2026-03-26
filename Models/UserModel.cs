using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ResearchProjectManagementSystem.Models
{
    [Table("Users")]
    public class UserModel
    {
        [Key]
        [Column("Id")]
        public int IdUser { get; set; }
        [Required]
        public string Username { get; set; }
        [Required]
        public string EmailAddress { get; set; }
        [Required]
        public string PasswordHash { get; set; }
        [Required]
        public string Salt { get; set; }
        [Required]
        public string DisplayName { get; set; }
        [Required]
        public DateTime RegisterDate { get; set; }
        [Required]
        public string UserRole { get; set; }
        public List<TeamMemberModel> Memberships { get; set; }
    }
}
