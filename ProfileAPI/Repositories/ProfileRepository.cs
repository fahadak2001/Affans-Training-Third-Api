using ProfileAPI.Data;
using ProfileAPI.Models;

namespace ProfileAPI.Repositories
{
    public class ProfileRepository : IProfileRepository
    {
        private readonly ProfileAPIDBContext _dbContext;
        public ProfileRepository(ProfileAPIDBContext dbContext) 
        {
            _dbContext = dbContext;
        }
        public Profile GetByEmail(string email)
        {
            var profile = _dbContext.Profile.Find(email);
            return profile;
        }
        public void Create(Profile profile)
        {
            _dbContext.Profile.Add(profile);
            _dbContext.SaveChanges();
        }

        public void Update(Profile profile)
        {
            _dbContext.Profile.Update(profile);
            _dbContext.SaveChanges();
        }

        public void Delete(string email)
        {
            var profileToDelete = _dbContext.Profile.Find(email);

            _dbContext.Profile.Remove(profileToDelete);
            _dbContext.SaveChanges();
        }
    }
}
