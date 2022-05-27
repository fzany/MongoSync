using System;
using MongoDB.Bson.Serialization.Attributes;
using Newtonsoft.Json;

namespace MongoSync.Models
{
    public class User
    {
        [BsonId]
        [JsonProperty("Id")]
        public string Id { get; set; }

        [JsonProperty("FirstName")]
        public string FirstName { get; set; }

        [JsonProperty("LastName")]
        public string LastName { get; set; }

        [JsonProperty("ProfilePhotoId")]
        public string ProfilePhotoId { get; set; }

        [JsonProperty("MutualFriendsCount")]
        public long MutualFriendsCount { get; set; }

        [JsonProperty("MutualGroupsCount")]
        public long MutualGroupsCount { get; set; }

        [JsonProperty("IsFriend")]
        public bool IsFriend { get; set; }

        [JsonProperty("City")]
        public string City { get; set; }

        [JsonProperty("State")]
        public string State { get; set; }

        [JsonProperty("Country")]
        public string Country { get; set; }

        [JsonProperty("ZipCode")]
        public string ZipCode { get; set; }

        [JsonProperty("UserMode")]
        public string UserMode { get; set; }

        [JsonProperty("NetworkType")]
        public int? NetworkType { get; set; }

        [JsonProperty("FIELD14")]
        public string FIELD14 { get; set; }
    }
}
