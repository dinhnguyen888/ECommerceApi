﻿using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
namespace ECommerceApi.Models
{
    public class Category
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; }
        [BsonElement("categoryName")]
        public string CategoryName { get; set; }
        [BsonElement("blockName")]
        public string? BlockName { get; set; }
        [BsonElement("frontendEndpoint")]
        public string FrontendEndpoint { get; set; }
    }
}
