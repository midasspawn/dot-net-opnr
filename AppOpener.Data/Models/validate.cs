using Amazon.Runtime.Internal.Endpoints.StandardLibrary;
using Microsoft.VisualBasic;
using System;
using System.Text.RegularExpressions;

namespace AppOpener.Data.Models
{

    public interface Ivalidate
    { 
    
    }
    public class validate
    {
        public validate()
        {

        }

        public static bool validURL(string str)
        {
            bool Result = false;
            try
            {
                Regex urlRx = new Regex(@"^(http|ftp|https|www)://([\w+?\.\w+])+([a-zA-Z0-9\~\!\@\#\$\%\^\&\*\(\)_\-\=\+\\\/\?\.\:\;\'\,]*)?$", RegexOptions.IgnoreCase);
                Regex pattern = new Regex(
        "^(https?:\\/\\/)?" + // protocol
          "((([a-z\\d]([a-z\\d-]*[a-z\\d])*)\\.)+[a-z]{2,}|" + // domain name
          "((\\d{1,3}\\.){3}\\d{1,3}))" + // OR ip (v4) address
          "(\\:\\d+)?(\\/[-a-z\\d%_.~+]*)*" + // port and path
          "(\\?[;&a-z\\d%_.~+=-]*)?" + // query string
          "(\\#[-a-z\\d_]*)?$"
      ); // fragment locator
                //return true;
                return urlRx.IsMatch(str);
            }
            catch (Exception ex)
            {
            }
            return Result;

        }


        public static bool validatePlatformUrl(PlatFormTag platform, string url_)
        {
            bool Result = false;
            Uri urlObject = new Uri(url_);
            string hostName_= urlObject.Host.ToLower();

            switch (platform)
            {
                case PlatFormTag.Youtube:
                    Result= hostName_.Contains("youtube") || urlObject.Host.Contains("youtu.be");
                    break;
                case PlatFormTag.Instagram:
                    Result = hostName_.Contains("instagram");
                    break;
                case PlatFormTag.Spotify:
                    Result = hostName_.Contains("spotify") ;
                    break;
                case PlatFormTag.Telegram:
                    Result = hostName_.Contains("telegram") || urlObject.Host.Contains("t.me");
                    break;
                case PlatFormTag.Twitter:
                    Result = hostName_.Contains("twitter") ;
                    break;
                case PlatFormTag.Linkedin:
                    Result = hostName_.Contains("linkedin") ;
                    break;
                case PlatFormTag.Playstore:
                    Result = hostName_.Contains("play.google") ;
                    break;
                default:
                    Result = false;
                    break;

            }
            return Result;
        }

        public PlatFormTag getPlatFormTag(string url_)
        {
            PlatFormTag platFormTagplatform = PlatFormTag.Other;
            Uri urlObject = new Uri(url_);
            string hostName_ = urlObject.Host.ToLower();
            try
            {
                if (hostName_.Contains(PlatFormInfo.Youtube) || urlObject.Host.Contains(PlatFormInfo.Youtube1))
                {
                    return platFormTagplatform = PlatFormTag.Youtube;
                }
                if (hostName_.Contains(PlatFormInfo.instagram) )
                {
                    return platFormTagplatform = PlatFormTag.Instagram;
                }
                if (hostName_.Contains(PlatFormInfo.spotify))
                {
                    return platFormTagplatform = PlatFormTag.Spotify;
                }
                if (hostName_.Contains(PlatFormInfo.telegram) || hostName_.Contains(PlatFormInfo.telegram1))
                {
                    return platFormTagplatform = PlatFormTag.Telegram;
                }
                if (hostName_.Contains(PlatFormInfo.twitter) )
                {
                    return platFormTagplatform = PlatFormTag.Twitter;
                }
                if (hostName_.Contains(PlatFormInfo.linkedin))
                {
                    return platFormTagplatform = PlatFormTag.Linkedin;
                }
                if (hostName_.Contains(PlatFormInfo.playstore))
                {
                    return platFormTagplatform = PlatFormTag.Playstore;
                }

            }
            catch (Exception ex)
            {

                
            }
            return platFormTagplatform;


        }

        public static string get_Tag(string url_)
        {
            string tag = "Other";
            Uri urlObject = new Uri(url_);
            string hostName_ = urlObject.Host.ToLower();
            try
            {
                if (hostName_.Contains(PlatFormInfo.Youtube) || urlObject.Host.Contains(PlatFormInfo.Youtube1))
                {
                    return tag = "Youtube";
                }
                if (hostName_.Contains(PlatFormInfo.instagram))
                {
                    return tag = "Instagram";
                }
                if (hostName_.Contains(PlatFormInfo.spotify))
                {
                    return tag = "Spotify";
                }
                if (hostName_.Contains(PlatFormInfo.telegram) || hostName_.Contains(PlatFormInfo.telegram1))
                {
                    return tag = "Telegram";
                }
                if (hostName_.Contains(PlatFormInfo.twitter))
                {
                    return tag = "Twitter";
                }
                if (hostName_.Contains(PlatFormInfo.linkedin))
                {
                    return tag = "Linkedin";
                }
                if (hostName_.Contains(PlatFormInfo.playstore))
                {
                    return tag = "Playstore";
                }

            }
            catch (Exception ex)
            {


            }
            return tag;


        }

    }


    public static  class PlatFormInfo
    {
        public const string Youtube = "youtube";
        public const string Youtube1 = "youtu.be";
        public const string instagram = "instagram";
        public const string spotify = "spotify";
        public const string telegram = "telegram";
        public const string telegram1 = "t.me";
        public const string twitter = "twitter";
        public const string linkedin = "linkedin";
        public const string playstore = "play.google";

    }
    public enum PlatFormTag
    {
        Youtube=0,
        Instagram=1,
        Spotify=2,
        Telegram=3,
        Twitter=4,
        Linkedin=5,
        Playstore=6,
        Other
    }

    


}
