using System;
using System.Collections.Generic;
using System.Text;

namespace AppOpener.Data.Models
{
    public class MongoDbDatabaseSettings
    {
        public string ConnectionString { get; set; }

        public string DatabaseName { get; set; }

        public string BasePlanCollectionName { get; set; }

        public string LinksCollectionName { get; set; }

        public string IntendListCollectionName { get; set; }

        public string GoogleUserCollectionName { get; set; }

        public string ClientCollectionName { get; set; }

        public string ClientTokenCollectionName { get; set; }

    }
}
