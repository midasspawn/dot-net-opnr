using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System;
using System.ComponentModel.DataAnnotations;

namespace AppOpener.Core.BusinessEntities.Client
{
    public class ClientTokenViewModel
    {
        [BsonId]
        public BsonObjectId _id { get; set; }

        public ClientTokenViewModel()
        {
			IssuedOn = DateTime.UtcNow;
        }
        public string ClientId { get; set; }
        public string AuthToken { get; set; }
        public DateTime IssuedOn { get; set; }
        public DateTime ExpiresOn { get; set; }
    }
}
