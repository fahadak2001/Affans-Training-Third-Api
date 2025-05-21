using ProfileAPI.Models;
namespace ProfileAPI.Services
{
    public interface IProfileService
    {
        Profile GetProfileByEmail(string email);
        void CreateProfile(Profile profile);
        void UpdateProfile(Profile profile);
        void DeleteProfile(string email);    }
}
