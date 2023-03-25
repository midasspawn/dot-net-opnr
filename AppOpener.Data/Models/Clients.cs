using System;
using System.Collections.Generic;

namespace AppOpener.Data.Models
{
    public partial class Clients
    {
        public Clients()
        {
            ClientTokens = new HashSet<ClientTokens>();
        }

        public long Id { get; set; }
        public string ClientName { get; set; }
        public string ClientKey { get; set; }
        public string ClientSecret { get; set; }

        public Clients IdNavigation { get; set; }
        public Clients InverseIdNavigation { get; set; }
        public ICollection<ClientTokens> ClientTokens { get; set; }
    }
}
