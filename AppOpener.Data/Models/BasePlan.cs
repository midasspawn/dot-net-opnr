using System;
using System.Collections.Generic;
using System.Text;

namespace AppOpener.Data.Models
{
   public class BasePlan
   {
        public BasePlan()
        {

        }

        public string email { get; set; }
        public string name { get; set; }

        public Dictionary<string,Int32> tag_count { get; set; }

        public string thumbnail { get; set; }

        public int total_count { get; set; }

        public string user_id { get; set; }
    }



}
