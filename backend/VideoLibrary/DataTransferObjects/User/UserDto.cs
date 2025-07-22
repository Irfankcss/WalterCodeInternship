using System.ComponentModel.DataAnnotations;

namespace VideoLibrary.DataTransferObjects.User
{
    public class UserDto
    {
        public int Id { get; set; }
        [Required(ErrorMessage = "Username is required")]
        public string Username { get; set; }
        [Required(ErrorMessage = "Email is required")]
        public string Email { get; set; }
        [Required(ErrorMessage ="Name is required")]
        public string Name { get; set; }
        [Required(ErrorMessage = "Date of birth is required")]
        public DateTime Dob { get; set; }
        [Required(ErrorMessage = "Admin status is required")]
        public bool Admin { get; set; }
    }

}
