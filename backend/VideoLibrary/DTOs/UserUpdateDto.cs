namespace VideoLibrary.Dtos
{
    public class UserUpdateDto
    {
        public string Username { get; set; }
        public string Password { get; set; }
        public string Email { get; set; }
        public string Name { get; set; }
        public DateTime Dob { get; set; }
        public bool Admin { get; set; }
    }
}
