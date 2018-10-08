namespace AngularWebsiteAPI.Dtos
{
    public class UserForLoginDto // doesn't need data validation
    {
        public string Username { get; set; }
        public string Password { get; set; }
    }
}