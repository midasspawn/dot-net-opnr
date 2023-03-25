using System;

namespace AppOpener.Data.Models
{
    public class helper
    {
        public helper()
        { 
        
        }
        public static string getidentify_tag(string short_tag)
        {
            string result=string.Empty; 
            try
            { 
                
                switch (short_tag.ToLower().Trim()) {

                    case "yt":
                        result = "Youtube";
                        break;
                    case "youtube":
                        result = "Youtube"; 
                        break;
                    case "ig":
                        result = "Instagram";
                        break;
                    case "instagram":
                        result = "Instagram";
                        break;
                    case "sp":
                        result = "Spotify";
                        break;
                    case "spotify":
                        result = "Spotify";
                        break;
                    case "tg":
                        result = "Telegram";
                        break;
                    case "telegram":
                        result = "Telegram";
                        break;
                    case "tw":
                        result = "Twitter";
                        break;
                    case "twitter":
                        result = "Twitter";
                        break;
                    case "lk":
                        result = "Linkedin";
                        break;
                    case "linkedin":
                        result = "Linkedin";
                        break;
                    case "ps":
                        result = "Playstore";
                        break;
                    case "playstore":
                        result = "Playstore";
                        break;
                    case "web":
                        result = "Other";
                        break;
                    case "url":
                        result = "Other";
                        break;
                    default:
                        result = string.Empty;
                        break;
                }
            
            }
            catch { }
            return result;
        }

        public static PlatFormTag getidentify_platformTag(string short_tag)
        {
            PlatFormTag platFormTagobj = PlatFormTag.Other;
            string result = string.Empty;
            try
            {

                switch (short_tag.ToLower().Trim())
                {

                    case "yt":
                        result = "Youtube";
                        platFormTagobj = PlatFormTag.Youtube;
                        break;
                    case "youtube":
                        result = "Youtube";
                        platFormTagobj = PlatFormTag.Youtube;
                        break;
                    case "ig":
                        result = "Instagram";
                        platFormTagobj = PlatFormTag.Instagram;
                        break;
                    case "instagram":
                        result = "Instagram";
                        platFormTagobj = PlatFormTag.Instagram;
                        break;
                    case "sp":
                        result = "Spotify";
                        platFormTagobj = PlatFormTag.Spotify;
                        break;
                    case "spotify":
                        result = "Spotify";
                        platFormTagobj = PlatFormTag.Spotify;
                        break;
                    case "tg":
                        result = "Telegram";
                        platFormTagobj = PlatFormTag.Telegram;
                        break;
                    case "telegram":
                        result = "Telegram";
                        platFormTagobj = PlatFormTag.Telegram;
                        break;
                    case "tw":
                        result = "Twitter";
                        platFormTagobj = PlatFormTag.Twitter;
                        break;
                    case "twitter":
                        result = "Twitter";
                        platFormTagobj = PlatFormTag.Twitter;
                        break;
                    case "lk":
                        result = "Linkedin";
                        platFormTagobj = PlatFormTag.Linkedin;
                        break;
                    case "linkedin":
                        result = "Linkedin";
                        platFormTagobj = PlatFormTag.Linkedin;
                        break;
                    case "ps":
                        result = "Playstore";
                        platFormTagobj = PlatFormTag.Playstore;
                        break;
                    case "playstore":
                        result = "Playstore";
                        platFormTagobj = PlatFormTag.Playstore;
                        break;
                    case "web":
                        result = "Other";
                        platFormTagobj = PlatFormTag.Other;
                        break;
                    case "url":
                        result = "Other";
                        platFormTagobj = PlatFormTag.Other;
                        break;
                    default:
                        result = string.Empty;
                        platFormTagobj = PlatFormTag.Other;
                        break;
                }

            }
            catch { }
            return platFormTagobj;
        }

    }

    public class urls
    {
        public urls() { }

        public dynamic findURL(string shortid)
        {
            dynamic result=null;
            try
            {



            }
            catch (Exception ex)
            {

                
            }
            return result;


        }


    }
}
