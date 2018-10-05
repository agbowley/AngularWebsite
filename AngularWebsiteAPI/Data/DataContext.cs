using AngularWebsite.API.Models;
using Microsoft.EntityFrameworkCore;

namespace AngularWebsite.API.Data
{
    public class DataContext : DbContext // datacontext model implements the dbcontext model
    {
        public DataContext(DbContextOptions<DataContext> options) : base (options) {} // datacontext constructor that takes a dbcontextoptions object as an overload and implements the base constructor with the same options
        public DbSet<Value> Values { get; set; } // add the values dbset to the datacontext
        public DbSet<User> Users { get; set; } // add the users dbset to the datacontext
    }
}