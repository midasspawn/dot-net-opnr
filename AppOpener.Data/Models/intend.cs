using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;

namespace AppOpener.Data.Models
{
    public class intend
    {
        public validate validateObj = new validate();
        public intend()
        {

        }

        public static void check_intend(string mobile_os, string devicetype, PlatFormTag platFormTag_, string url)
        {
            try
            {

                switch (platFormTag_)
                {
                    case PlatFormTag.Youtube:


                        break;
                    default:
                        break;

                }
            }
            catch (Exception ex)
            {


            }


        }

        public static intendResponse get_PlatFormintend(Dictionary<PlatFormTag, IntendList> intentlistobj, PlatFormTag platFormTag_, string mobile_os, string devicetype, string url_)
        {
            intendResponse intendResponseobj = new intendResponse();
            mobile_os = mobile_os.ToLower();
            devicetype = devicetype.ToLower();
            string os_type = string.Empty;
            string app_intend = string.Empty;
            try
            {
                var Tag_INTENDS = intentlistobj.ContainsKey(platFormTag_) ? intentlistobj[platFormTag_] : null;

                if (Tag_INTENDS != null)
                {
                    // For Youtube Intend
                    if (platFormTag_.Equals(PlatFormTag.Youtube))
                    {
                        if (validate.validatePlatformUrl(platFormTag_, url_))
                        {
                            var intend = url_.Split(new string[] { "//" }, StringSplitOptions.None);
                            string pure_intend = intend[1];
                            if (Tag_INTENDS != null)
                            {
                                if (mobile_os == "android")
                                {
                                    app_intend = Tag_INTENDS.intend_android_before + pure_intend + Tag_INTENDS.intend_android_after;
                                    os_type = "android";
                                }
                                else if (mobile_os == "ios")
                                {
                                    app_intend = Tag_INTENDS.intend_ios_before + pure_intend + Tag_INTENDS.intend_ios_after;
                                    os_type = "ios";
                                }
                                else if (mobile_os == "windows" || devicetype == "desktop")
                                {
                                    os_type = "windows";
                                    app_intend = url_;
                                }
                                else
                                {
                                    os_type = "windows";
                                    app_intend = url_;
                                }
                            }
                            else
                            {
                                os_type = "windows";
                                app_intend = url_;
                            }
                        }
                        else
                        {
                            os_type = "windows";
                            app_intend = url_;
                        }
                    }

                    // For Instagram Intend
                    if (platFormTag_.Equals(PlatFormTag.Instagram))
                    {
                        if (validate.validatePlatformUrl(platFormTag_, url_))
                        {
                            var intend = url_.Split(new string[] { "//" }, StringSplitOptions.None);
                            string pure_intend = intend[1];
                            if (Tag_INTENDS != null)
                            {
                                if (mobile_os == "android")
                                {
                                    app_intend = Tag_INTENDS.intend_android_before + pure_intend + Tag_INTENDS.intend_android_after;
                                    os_type = "android";
                                }
                                else if (mobile_os == "ios")
                                {
                                    var more_split = pure_intend.Split("/");
                                    app_intend = Tag_INTENDS.intend_ios_before + more_split[1] + Tag_INTENDS.intend_ios_after;
                                    os_type = "ios";
                                }
                                else if (mobile_os == "windows" || devicetype == "desktop")
                                {
                                    os_type = "windows";
                                    app_intend = url_;
                                }
                                else
                                {
                                    os_type = "windows";
                                    app_intend = url_;
                                }

                            }
                            else
                            {
                                os_type = "windows";
                                app_intend = url_;
                            }
                        }
                        else
                        {
                            os_type = "windows";
                            app_intend = url_;
                        }
                    }

                    // For Spotify Intend
                    if (platFormTag_.Equals(PlatFormTag.Spotify))
                    {
                        //var url_split = url.parse(og_url, true);
                        //const spotify_path = url_split.pathname;
                        var url_split = new Uri(url_);
                        string spotify_path = url_split.Host;
                        if (mobile_os == "android")
                        {
                            // console.log("mobile - android");
                            app_intend = Tag_INTENDS.intend_android_before + spotify_path;
                            os_type = "android";

                        }
                        else if (mobile_os == "ios")
                        {
                            //console.log("mobile - ios");
                            app_intend = Tag_INTENDS.intend_ios_before + spotify_path;
                            os_type = "ios";
                        }
                        else if (mobile_os == "windows" || devicetype == "desktop")
                        {
                            //os_type = "windows";
                            app_intend = url_;
                        }
                        else
                        {
                            os_type = "windows";
                            app_intend = url_;
                        }

                    }

                    // For Telegram Intend

                    if (platFormTag_.Equals(PlatFormTag.Telegram))
                    {
                        var url_split = new Uri(url_);
                        string telegram_path = url_split.Host; // remove / from /s/xxxxx
                        telegram_path = telegram_path.Substring(1);
                        //console.log(telegram_path);
                        if (mobile_os == "android")
                        {
                            app_intend = Tag_INTENDS.intend_android_before + telegram_path + Tag_INTENDS.intend_android_after;
                            os_type = "android";
                        }
                        else if (mobile_os == "ios")
                        {
                            app_intend = Tag_INTENDS.intend_ios_before + telegram_path + Tag_INTENDS.intend_ios_after;
                            os_type = "ios";
                        }
                        else if (mobile_os == "windows" || devicetype == "desktop")
                        {
                            os_type = "windows";
                            app_intend = url_;
                        }
                        else
                        {
                            os_type = "windows";
                            app_intend = url_;
                        }
                    }

                    // For Twitter Intend
                    if (platFormTag_.Equals(PlatFormTag.Twitter))
                    {
                        var url_split = new Uri(url_);
                        string twitter_path = url_split.Host;

                        if (mobile_os == "android")
                        {
                            app_intend = Tag_INTENDS.intend_android_before + twitter_path + Tag_INTENDS.intend_android_after;
                            os_type = "android";
                        }
                        else if (mobile_os == "ios")
                        {
                            app_intend = "";
                            if (twitter_path.Contains("status"))
                            {
                                //this for ios status
                                string last_idofurl = twitter_path.Split("/").LastOrDefault();
                                twitter_path = "status?id=" + last_idofurl;
                                //console.log("ios - status = " + twitter_path);
                                app_intend = Tag_INTENDS.intend_ios_before + twitter_path + Tag_INTENDS.intend_ios_after;
                            }
                            else
                            {
                                twitter_path = "user?screen_name=" + twitter_path.Substring(1);
                                //console.log("ios - profile = " + twitter_path);
                                app_intend = Tag_INTENDS.intend_ios_before + twitter_path + Tag_INTENDS.intend_ios_after;

                            }
                            os_type = "ios";
                        }
                        else if (mobile_os == "windows" || devicetype == "desktop")
                        {
                            os_type = "windows";
                            app_intend = url_;
                        }
                        else
                        {
                            os_type = "windows";
                            app_intend = url_;
                        }
                    }

                    // For Linkedin Intend
                    if (platFormTag_.Equals(PlatFormTag.Linkedin))
                    {
                        var url_split = new Uri(url_);
                        string linkedin_path = url_split.Host;
                        //console.log(linkedin_path);
                        if (mobile_os == "android")
                        {
                            app_intend = Tag_INTENDS.intend_android_before + linkedin_path + Tag_INTENDS.intend_android_after;
                            os_type = "android";
                        }
                        else if (mobile_os == "ios")
                        {
                            linkedin_path = linkedin_path.Substring(1);
                            //console.log("ios - profile = " + twitter_path);
                            app_intend = Tag_INTENDS.intend_ios_before + linkedin_path + Tag_INTENDS.intend_ios_after;
                            os_type = "ios";
                        }
                        else if (mobile_os == "windows" || devicetype == "desktop")
                        {
                            os_type = "windows";
                            app_intend = url_;
                        }
                        else
                        {
                            os_type = "windows";
                            app_intend = url_;
                        }
                    }

                    // For Playstore Intend

                    if (platFormTag_.Equals(PlatFormTag.Playstore))
                    {
                        var intend = url_.Split("=");
                        string pure_intend = intend[1];
                        //console.log(pure_intend);
                        if (mobile_os == "android")
                        {
                            app_intend = Tag_INTENDS.intend_android_before + pure_intend + Tag_INTENDS.intend_android_after;
                            os_type = "android";
                        }
                        else if (mobile_os == "ios")
                        {
                            app_intend = url_;
                            os_type = "ios";
                        }
                        else if (mobile_os == "windows" || devicetype == "desktop")
                        {
                            os_type = "windows";
                            app_intend = url_;
                        }
                        else
                        {
                            os_type = "windows";
                            app_intend = url_;
                        }

                    }


                }
                else
                {
                    // For Other Intend
                    os_type = "windows";
                    app_intend = url_;
                }


                intendResponseobj = new intendResponse(app_intend, os_type);

            }
            catch (Exception ex)
            {

            }
            return intendResponseobj;
        }
    }

    public class intendResponse
    {
        public intendResponse()
        {

        }

        public intendResponse(string app_intend_, string os_type_)
        {
            this.app_intend = app_intend_;
            this.os_type = os_type_;
        }

        public string app_intend { get; set; }

        public string os_type { get; set; }

    }
}
