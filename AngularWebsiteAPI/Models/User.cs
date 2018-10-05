namespace AngularWebsite.API.Models
{
    public class User 
    {
        public int Id { get; set; } // user id
        public string Username { get; set; } // user name
        public byte[] PasswordHash {get;set; } // user password hash
        public byte[] PasswordSalt { get; set; } // user password salt, passwords are never stored in plaintext
    }
}