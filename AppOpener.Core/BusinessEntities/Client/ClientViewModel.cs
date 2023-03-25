using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace AppOpener.Core.BusinessEntities.Client
{
    public class ClientViewModel
	{
        [BsonId]
        public BsonObjectId _id { get; set; }
		public Int64 ClientId { get; set; }
		public string ClientName { get; set; }
		public string ClientKey { get; set; }
		public string ClientSecret { get; set; }
    }
	public class ClientModel
	{
		[Required(ErrorMessage ="Client key is required.")]
		public string ClientKey { get; set; }
		[Required(ErrorMessage = "Client secret is required.")]
		public string ClientSecret { get; set; }
	}
}
