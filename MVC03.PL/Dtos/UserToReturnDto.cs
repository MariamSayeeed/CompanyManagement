namespace MVC03.PL.Dtos
{
    public class UserToReturnDto
    {
        public string Id { get; set; }
        public string UserName { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }

        public IEnumerable<string>? Roles { get; set; }

    }
}
