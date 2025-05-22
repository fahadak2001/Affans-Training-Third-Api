using ProfileAPI.Models;

namespace ProfileAPI.Repositories
{
    public interface IProfileRepository
    {
        Profile GetByEmail(string email);
        void Create(Profile profile);
        void Update(Profile profile);
        void Delete(string email);
    }
}
