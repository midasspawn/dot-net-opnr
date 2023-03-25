using System;
using System.Collections.Generic;
using System.Text;

namespace AppOpener.Data.Models  
{
    public class IntendList
    {
        public IntendList()
        {

        }

        public string id { get; set; }

        public string Popular { get; set; }
        public string Social { get; set; }
        public string INTEND_ANDROID { get; set; }
        public string INTEND_IOS { get; set; }

        public string intend_android_after { get; set; }

        public string intend_android_before { get; set; }

        public string intend_ios_after { get; set; }
        public string intend_ios_before { get; set; }


    }

}
