using System;
using System.Collections.Generic;

namespace AppOpener.Data.Models
{
    public partial class ClientTokens
    {
        public long Id { get; set; }
        public long ClientId { get; set; }
        public string AuthToken { get; set; }
        public DateTime IssuedOn { get; set; }
        public DateTime ExpiresOn { get; set; }

        public Clients Client { get; set; }
    }
}
