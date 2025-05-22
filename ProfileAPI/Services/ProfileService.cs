using ProfileAPI.Models;
using ProfileAPI.Repositories;

namespace ProfileAPI.Services
{
    public class ProfileService : IProfileService
    {
        private readonly IProfileRepository _profileRepository;
        private readonly IConfiguration _configuration;
        public ProfileService(IProfileRepository profileRepository, IConfiguration configuration)
        {
            _profileRepository = profileRepository;
            _configuration = configuration;
        }
        public Profile GetProfileByEmail(string email)
        {
            return _profileRepository.GetByEmail(email);
        }
        public void CreateProfile(Profile profile)
        {
            _profileRepository.Create(profile);
        }
        public void UpdateProfile(Profile profile)
        {
            _profileRepository.Update(profile);
        }
        public void DeleteProfile(string email)
        {
            _profileRepository.Delete(email);
        }
    }
    
}
