using System;
using System.Collections.Generic;
using System.Text;

namespace AppOpener.Data.Models
{
    public class Links
    {
        public Links()
        {

        }

        public Links(string originalURL_,string tag_, string shortid_, string userid_)
        {
            this.created_at = DateTime.Now;
            this._id = shortid_;
            this.originalURL = originalURL_;
            this.tag = tag_;
            this.user_id = userid_;
        }

        public int click_count { get; set; }

        public DateTime ?created_at { get; set; }

        public string _id { get; set; }

        public string  originalURL { get; set; }

        public string tag { get; set; }

        public string user_id { get; set; }


    }
}
