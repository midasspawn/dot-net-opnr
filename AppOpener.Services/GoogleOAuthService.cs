using AppOpener.Data.Models;
using MongoDB.Driver;
using System.Net;
using Newtonsoft.Json;
using Microsoft.Extensions.Options;
using RestSharp;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.IO;
using System;
using Google.Apis.Auth;

namespace AppOpener.Services
{

    public interface IGoogleOAuthService
    {
        Task<string> GetToken(string code);

        string GetuserProfile(string accesstoken);

        Task<List<GoogleUserclass>> GetAllGoogleUsers();

        Task<List<GoogleUserclass>> GetGoogleUsersBycheckUserExist(string userid_, string name_, string email_);

        Task<GoogleUserclass> GetGoogleUsersByUserId(string userid);

        Task<GoogleUserclass> CreateGoogleUsers(GoogleUserclass googleUserclass);

        Task UpdateGoogleUsers(string userid, GoogleUserclass googleUserclass);

        Task RemoveGoogleUsers(string userid);

        Task<GoogleJsonWebSignature.Payload> GoogleIdTokenVerifier(string tokenID);

    }

    public class GoogleOAuthService : IGoogleOAuthService
    {
        private readonly GoogleCredentials _googleCredentials;
        private string oauth2url = "https://accounts.google.com/o/oauth2/token";
        private string redirection_url = "http://localhost:5265/api/LoginWithGoogle/GetToken.aspx";
        private readonly IMongoCollection<Book> _booksCollection;

        private readonly IMongoCollection<GoogleUserclass> _UserclassCollection;
        public GoogleOAuthService(IOptions<GoogleCredentials> googleCredentials, IOptions<MongoDbDatabaseSettings> mongoDbDatabaseSettings) {
        this._googleCredentials = googleCredentials.Value;
            var mongoClient = new MongoClient(
                mongoDbDatabaseSettings.Value.ConnectionString);

            var mongoDatabase = mongoClient.GetDatabase(
                mongoDbDatabaseSettings.Value.DatabaseName);

            _UserclassCollection = mongoDatabase.GetCollection<GoogleUserclass>(
                mongoDbDatabaseSettings.Value.GoogleUserCollectionName);
        }

        public async Task<string> GetToken(string code)
        {
            string result=string.Empty;
            try
            {
                string poststring = "grant_type=authorization_code&code=" + code + "&client_id=" + _googleCredentials.ClientID + "&client_secret=" + _googleCredentials.Clientsecret + "&redirect_uri=" + redirection_url + "";
                var options = new RestClientOptions("https://accounts.google.com/o/oauth2/token")
                {
                    MaxTimeout = -1,
                };
                var client = new RestClient(options);
                var request = new RestRequest("https://accounts.google.com/o/oauth2/token", Method.Post);
                request.AddHeader("Content-Type", "application/x-www-form-urlencoded");
                request.AddParameter("grant_type", "authorization_code");
                request.AddParameter("code", code);
                request.AddParameter("client_id", _googleCredentials.ClientID);
                request.AddParameter("client_secret", _googleCredentials.Clientsecret);
                request.AddParameter("redirect_uri", redirection_url);
                RestResponse response = client.Execute(request);
                var obj = JsonConvert.DeserializeObject<Tokenclass>(response.Content);
                result = GetuserProfile(obj.access_token);
                var user = JsonConvert.DeserializeObject<GoogleUserclass>(result);
                await CreateGoogleUsers(user);
            }
            catch(Exception ex)
            {

            }
            return result;
        }

        public string GetuserProfile(string accesstoken)
        {
            dynamic Result = null;
            string url = "https://www.googleapis.com/oauth2/v1/userinfo?alt=json&access_token=" + accesstoken + "";
            WebRequest request = WebRequest.Create(url);
            request.Credentials = CredentialCache.DefaultCredentials;
            WebResponse response = request.GetResponse();
            Stream dataStream = response.GetResponseStream();
            StreamReader reader = new StreamReader(dataStream);
            string responseFromServer = reader.ReadToEnd();

            
            return Result= responseFromServer;
        }


        public async Task<List<GoogleUserclass>> GetAllGoogleUsers()
        {
            return await _UserclassCollection.Find(_ => true).ToListAsync();
        }

        public async Task<List<GoogleUserclass>> GetGoogleUsersBycheckUserExist(string userid_,string name_,string email_)
        {
            return await _UserclassCollection.Find(x => x.id == userid_ || x.name== name_|| x.email==email_).ToListAsync();
        }

        public async Task<GoogleUserclass> GetGoogleUsersByUserId(string userid)
        {
            return await _UserclassCollection.Find(x => x.id == userid).FirstOrDefaultAsync();
        }

        public async Task<GoogleUserclass> CreateGoogleUsers(GoogleUserclass googleUserclass)
        {
            var Insertobject= _UserclassCollection.InsertOneAsync(googleUserclass);
            Insertobject.Wait();
            return await _UserclassCollection.Find(x => x.id == googleUserclass.id).FirstOrDefaultAsync(); ;
        }

        public async Task UpdateGoogleUsers(string userid,GoogleUserclass googleUserclass)
        {
            await _UserclassCollection.ReplaceOneAsync(x => x.id == userid, googleUserclass);
            
        }

        public async Task RemoveGoogleUsers(string userid)
        {
            await _UserclassCollection.DeleteOneAsync(x => x.id == userid);
        }

        public async Task<GoogleJsonWebSignature.Payload> GoogleIdTokenVerifier(string tokenID)
        {
            GoogleJsonWebSignature.Payload payload = new GoogleJsonWebSignature.Payload();
            try
            {

                payload = await GoogleJsonWebSignature.ValidateAsync(tokenID, new GoogleJsonWebSignature.ValidationSettings
                {
                    Audience = new[] { _googleCredentials.ClientID }
                });
            }
            catch
            {


            }
            return payload;

        }


    }
}
