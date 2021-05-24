using System.ComponentModel.DataAnnotations;


namespace Assigment3.Models
{
    public class UserLoginDto
    {
        [Key]
        public int UserId { get; set; }
        [MaxLength(50)]
        public string Name { get; set; }
        [MaxLength(100)]
        public string UserPassword { get; set; }
        [MaxLength(100)]
        public string Email { get; set; }
        
    }
}
