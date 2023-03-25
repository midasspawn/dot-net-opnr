namespace AppOpener.Data.Models
{
    public class GoogleOAuth
    {
    }

    public class Tokenclass
    {
        public string access_token
        {
            get;
            set;
        }
        public string token_type
        {
            get;
            set;
        }
        public int expires_in
        {
            get;
            set;
        }
        public string refresh_token
        {
            get;
            set;
        }
    }

    public class GoogleUserclass
    {

        public GoogleUserclass()
        { 
        
        }
        public string id { get; set; }
        public string email { get; set; }
        public bool verified_email { get; set; }
        public string name { get; set; }
        public string given_name { get; set; }
        public string family_name { get; set; }
        public string picture { get; set; }
        public string locale { get; set; }
    }

    public class checkGoogleUserReq
    {
        public checkGoogleUserReq()
        { 
        }
        public string name { get; set; }
        public string email { get; set; }
        public string userid { get; set; }

    }
}
