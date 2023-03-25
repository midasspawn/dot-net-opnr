using System;
using System.Collections.Generic;

namespace AppOpener.Data.Models
{
    public partial class Settings
    {
        public long SettingId { get; set; }
        public string Name { get; set; }
        public string Value { get; set; }
        public DateTime CreatedOn { get; set; }
    }
}
