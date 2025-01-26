using System.IdentityModel.Tokens.Jwt;
using MongoDB.Driver;
using ECommerceApi.Models;

namespace ECommerceApi.Services
{
    public class UserProfileService : IUserProfileService
    {
        private readonly IMongoCollection<UserProfile> _userProfiles;
        private readonly JwtSecurityTokenHandler _tokenHandler;

        public UserProfileService(IMongoDatabase database)
        {
            _userProfiles = database.GetCollection<UserProfile>("UserProfiles");
            _tokenHandler = new JwtSecurityTokenHandler();
        }

        // Get My Profile
        public async Task<UserProfile?> GetMyProfileAsync(string token)
        {
            var userId = ValidateTokenAndGetUserId(token);
            if (userId == null) return null;

            return await _userProfiles.Find(profile => profile.UserId == userId).FirstOrDefaultAsync();
        }

        // Post My Profile
        public async Task<UserProfile> PostMyProfileAsync(UserProfile userProfile)
        {
            if (string.IsNullOrEmpty(userProfile.UserId))
                throw new ArgumentException("UserId is required to create a profile.");

            // Default values if fields are null
            userProfile.Username ??= string.Empty;
            userProfile.Avatar ??= null;
            userProfile.Birthday ??= null;
            userProfile.Address ??= null;

            await _userProfiles.InsertOneAsync(userProfile);
            return userProfile;
        }

        // Update My Profile
        public async Task<UserProfile?> UpdateMyProfileAsync(string userId, UserProfile updatedProfile)
        {
            var existingProfile = await _userProfiles.Find(profile => profile.UserId == userId).FirstOrDefaultAsync();
            if (existingProfile == null) return null;

            // Update only non-null fields
            existingProfile.Username = updatedProfile.Username ?? existingProfile.Username;
            existingProfile.Avatar = updatedProfile.Avatar ?? existingProfile.Avatar;
            existingProfile.Birthday = updatedProfile.Birthday ?? existingProfile.Birthday;
            existingProfile.Address = updatedProfile.Address ?? existingProfile.Address;

            await _userProfiles.ReplaceOneAsync(profile => profile.UserId == userId, existingProfile);
            return existingProfile;
        }

        // Delete My Profile
        public async Task<bool> DeleteMyProfileAsync(string userId)
        {
            var result = await _userProfiles.DeleteOneAsync(profile => profile.UserId == userId);
            return result.DeletedCount > 0;
        }

        // Helper: Validate Token and Extract UserId
        private string? ValidateTokenAndGetUserId(string token)
        {
            var jwtToken = _tokenHandler.ReadJwtToken(token);
            var userIdClaim = jwtToken.Claims.FirstOrDefault(claim => claim.Type == "userId");
            return userIdClaim?.Value;
        }
    }
}
