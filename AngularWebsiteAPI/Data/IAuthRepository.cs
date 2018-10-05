using System.Threading.Tasks;
using AngularWebsite.API.Models;

namespace AngularWebsiteAPI.Data
{
    public interface IAuthRepository // authrepository interface for relaying database queries between the middleware and the data layer
    {
        Task<User> Register(User user, string password); // instantiate register method with overloads as an async task
        Task<User> Login(string username, string password); // instantiate login method with overloads as an async task
        Task<bool> UserExists(string username); // instantiate userexists method with overloads as an async task
    }
}