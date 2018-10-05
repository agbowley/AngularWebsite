using System;
using System.Threading.Tasks;
using AngularWebsite.API.Data;
using AngularWebsite.API.Models;
using Microsoft.EntityFrameworkCore;

namespace AngularWebsiteAPI.Data
{
    public class AuthRepository : IAuthRepository // authrepository implements the iauthrepository interface
    {
        private readonly DataContext _context; // private readonly variable _context
        public AuthRepository(DataContext context) // constructor for the auth repository, accepts 1 overload dbcontext
        {
            _context = context; // intitialise the _context
        }
        public async Task<User> Login(string username, string password) // user login
        {
            var user = await _context.Users.FirstOrDefaultAsync(x => x.Username == username); // check if a user with a matching username exists in the database (FirstOrDefaultAsync returns null instead of an exception if nothing is returned)
            
            if (user == null) // if no matching user, return to controller
                return null;
            
            if (!VerifyPasswordHash(password, user.PasswordHash, user.PasswordSalt)) // check if the password that was entered matches the user's passwordHash/passwordSalt
                return null;

            return user; // return the user object to the controller method if username and password match
        }

        private bool VerifyPasswordHash(string password, byte[] passwordHash, byte[] passwordSalt) // verify password by comparing it to user's hash and salt
        {
            using (var hmac = new System.Security.Cryptography.HMACSHA512(passwordSalt)) //anything inside here is disposed of after use as the HMACSHA512 implements a dispose method
            {
                var computedHash = hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(password)); // hash the password
                for (int i=0;i<computedHash.Length;i++) // for each character in the password hash byte array
                {
                    if (computedHash[i] != passwordHash[i]) return false; // if the character at this position does not match the corresponding character in the other array, return false
                        return true; // if the hashes are the same, return true
                }
                return false; // otherwise return false
            }
        }

        public async Task<User> Register(User user, string password) // register user
        {
            byte[] passwordHash, passwordSalt; // create two empty byte arrays
            CreatePasswordHash(password, out passwordHash, out passwordSalt); // create password hash using out, passes the reference to password hash/salt in memory instead of the actual values

            user.PasswordHash = passwordHash; // set the user's password hash to the generated password hash
            user.PasswordSalt = passwordSalt; // set the user's password salt to the generated password salt

            await _context.Users.AddAsync(user); // asynchronously add the user to the dbcontext
            await _context.SaveChangesAsync(); // asynchronously save the changes to the dbcontext

            return user; // return the user object
        }

        private void CreatePasswordHash(string password, out byte[] passwordHash, out byte[] passwordSalt)
        {
            using (var hmac = new System.Security.Cryptography.HMACSHA512()) // anything inside here is disposed of after use as the HMACSHA512 implements a dispose method
            {
                passwordSalt = hmac.Key; // using the HMACSHA512 method, create the random key and use this as the password salt
                passwordHash = hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(password)); // hash the password that the user typed and store it as the password hash
            }
        }

        public async Task<bool> UserExists(string username) // check if the user exists
        {
            if (await _context.Users.AnyAsync(x => x.Username == username)) // asynchronously check if a user (x) has a matching username
                return true;
            
            return false;
        }
    }
}