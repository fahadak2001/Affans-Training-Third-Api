using Microsoft.AspNetCore.Authorization;
using ProfileAPI.Models;
using ProfileAPI.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Distributed;
using System.Text.Json;
using System.Text;

namespace ProfileAPI.Controllers
{
    [ApiController]
    [Route("api/profile")]
    [Produces("application/json")]
    public class ProfileController : Controller
    {

        private readonly IProfileService _profileService;
        private readonly IDistributedCache _distributedCache;
        public ProfileController(IProfileService profileService, IDistributedCache distributedCache)
        {
            _profileService = profileService;
            _distributedCache = distributedCache;
        }


        
        [HttpGet("Profile")]
        [Authorize]
        public async Task<ActionResult<Profile>> GetProfileAsync([FromBody] string email)
        {

            if (string.IsNullOrEmpty(email))
            {
                return BadRequest("Email is null or empty.");
            }
            string cacheKey = $"profile:{email}";
            byte[]? cachedProfileBytes = await _distributedCache.GetAsync(cacheKey);

            if (cachedProfileBytes != null)
            {
                var cachedProfile = JsonSerializer.Deserialize<List<Profile>>(Encoding.UTF8.GetString(cachedProfileBytes));
                return Ok(cachedProfile);
            }
            var profile = _profileService.GetProfileByEmail(email);
            return Ok(profile);
        }

        [HttpPost("Create")]
        [Authorize]
        public ActionResult<Profile> CreateProfile([FromBody] Profile profile)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            if ( (_profileService.GetProfileByEmail(profile.Email)) != null)
            {
                return BadRequest(("Profile with same Email already exists"));
            }
            

            _profileService.CreateProfile(profile);


            string cacheKey = $"profile:{profile.Email}";
            _distributedCache.RemoveAsync(cacheKey);
            return Ok(profile);
        }


        [HttpPut("Update")]
        [Authorize]
        public IActionResult UpdateProfile([FromBody] Profile profile)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var existingProfile = _profileService.GetProfileByEmail(profile.Email);
            if (existingProfile == null)
            {
                return NotFound();
            }

            existingProfile.Email = profile.Email;
            existingProfile.FirstName = profile.FirstName;
            existingProfile.LastName = profile.LastName;
            existingProfile.Number = profile.Number;
            existingProfile.Data = profile.Data;

            _profileService.UpdateProfile(existingProfile);

            string cacheKey = $"profile:{"profile.Email"}";
            _distributedCache.RemoveAsync(cacheKey);
            return Ok(existingProfile);
        }

        [HttpDelete("Delete/{email}")]
        [Authorize]
        public IActionResult DeleteProfile(string email)
        {
            var profileToDelete = _profileService.GetProfileByEmail(email);
            if (profileToDelete == null)
            {
                return NotFound();
            }

            _profileService.DeleteProfile(email);

            string cacheKey = $"profile:{email}";
            _distributedCache.RemoveAsync(cacheKey);
            return Ok();
        }

    }
}
