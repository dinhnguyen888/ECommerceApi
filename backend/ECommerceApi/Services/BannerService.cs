using AutoMapper;
using ECommerceApi.Dtos;
using ECommerceApi.Interfaces;
using ECommerceApi.Models;
using MongoDB.Bson;
using MongoDB.Driver;

namespace ECommerceApi.Services
{
    public class BannerService : IBannerService
    {
        private readonly IMongoCollection<Banner> _banners;
        private readonly IMapper _mapper;

        public BannerService(MongoDbContext dbContext, IMapper mapper)
        {
            _banners = dbContext.GetCollection<Banner>("Banner");
            _mapper = mapper;
        }

        public async Task<List<Banner>> GetAllBanner()
        {
            return await _banners.Find(_ => true).ToListAsync();
        }

        public async Task<List<Banner>> AddBaneer(BannerPostDto bannerDto)
        {
         
            var banner = _mapper.Map<Banner>(bannerDto);
            banner.Id = ObjectId.GenerateNewId().ToString();

            await _banners.InsertOneAsync(banner);
            return await GetAllBanner();
        }

        public async Task<bool> UpdateBaneer(string id, BannerUpdateDto bannerDto)
        {
            var updatedBanner = _mapper.Map<Banner>(bannerDto);
            updatedBanner.Id = id; 

            var filter = Builders<Banner>.Filter.Eq(b => b.Id, id);
            var updateResult = await _banners.ReplaceOneAsync(filter, updatedBanner);
            return updateResult.MatchedCount > 0;
        }

        public async Task<bool> DeleteBaneer(string id)
        {
            var result = await _banners.DeleteOneAsync(b => b.Id == id);
            return result.DeletedCount > 0;
        }
    }
}
