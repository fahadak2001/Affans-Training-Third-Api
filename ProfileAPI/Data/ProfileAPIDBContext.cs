using Microsoft.EntityFrameworkCore;
using ProfileAPI.Models;

namespace ProfileAPI.Data
{
    public class ProfileAPIDBContext : DbContext
    {
        public DbSet<Profile> Profile { get; set; }

        public ProfileAPIDBContext(DbContextOptions<ProfileAPIDBContext> options) : base(options)
        {
        }
    }
}
