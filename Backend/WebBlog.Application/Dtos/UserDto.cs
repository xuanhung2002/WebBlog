namespace WebBlog.Application
{
    public class UserDto : DtoBase
    {
        public string UserName { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string FullName => $"{FirstName + LastName}";
        public string Email { get; set; }
    }
}
