using ProfileAPI.Models;
using Microsoft.Extensions.Caching.Distributed;
using System.Text;
using System.Text.Json;


namespace ProfileAPI.Services
{

    public class CachedProfileService : ICachedProfileService
    {
        private readonly IProfileService _decoratedProfileService;
        private readonly IDistributedCache _distributedCache;
        private readonly DistributedCacheEntryOptions _cacheOptions;
        public CachedProfileService(IProfileService decoratedProfileService, IDistributedCache distributedCache)
        {
            _decoratedProfileService = decoratedProfileService;
            _distributedCache = distributedCache;
            _cacheOptions = new DistributedCacheEntryOptions()
                .SetSlidingExpiration(TimeSpan.FromMinutes(5));
        }

        public Profile GetProfileByEmail(string email)
        {
            string cacheKey = $"profile:{email}";
            byte[]? cachedProfileBytes = _distributedCache.Get(cacheKey);
            
            if (cachedProfileBytes != null)
            {
                return JsonSerializer.Deserialize<Profile>(Encoding.UTF8.GetString(cachedProfileBytes));
            }
            Profile profile = _decoratedProfileService.GetProfileByEmail(email);
            if (profile != null)
            {
                var profileJson = JsonSerializer.Serialize(profile);
                var profileBytes = Encoding.UTF8.GetBytes(profileJson);
                _distributedCache.Set(cacheKey, profileBytes, _cacheOptions);
            }
            return profile;
        }
        public void CreateProfile(Profile profile)
        {
            _decoratedProfileService.CreateProfile(profile);

            _distributedCache.Remove($"user:email:{profile.Email}");
            _distributedCache.Remove($"user:email:{profile.FirstName}");
            _distributedCache.Remove($"user:username:{profile.LastName}");
            _distributedCache.Remove($"user:username:{profile.Number}");
            _distributedCache.Remove($"user:username:{profile.Data}");

        }

        public void UpdateProfile(Profile profile)
        {
            _decoratedProfileService.UpdateProfile(profile);

            _distributedCache.Remove($"user:email:{profile.Email}");
            _distributedCache.Remove($"user:email:{profile.FirstName}");
            _distributedCache.Remove($"user:username:{profile.LastName}");
            _distributedCache.Remove($"user:username:{profile.Number}");
            _distributedCache.Remove($"user:username:{profile.Data}");
        }
        public void DeleteProfile(string email)
        {
            _decoratedProfileService.DeleteProfile(email);

            _distributedCache.Remove($"user:email:{email}");
        }
    }
}
