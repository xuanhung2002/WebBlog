namespace WebBlog.Application.Dto
{
    public class CreateUserRequest
    {
        public string UserName { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string FullName => $"{FirstName + LastName}";
        public string Email { get; set; }
        public string Password { get; set; }
    }
}
