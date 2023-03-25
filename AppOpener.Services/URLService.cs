using AppOpener.Data.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Options;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace AppOpener.Services
{

    public interface IURLService
    {

        Links findURL(string shortid);

        string Generate_shortrandomid_url();

        Task UpdateURLHit_count_increment(string shortid);

        Task<Links> CreateURL(string originalURL, string tag, string shortid, string userid);

        Task<GotoRes> GetSmartURL(GotoReq req, HttpRequest reqHeaders);

        void SaveIntendList();

        List<IntendList> GetAllIntendList();

        Dictionary<PlatFormTag, IntendList> GetDicIntendList();
    }

    public class URLService : IURLService
    {
        private readonly IMongoCollection<BasePlan> _basePlanCollection;
        private readonly IMongoCollection<Links> _linksCollection;
        private readonly IMongoCollection<IntendList> _IntendListCollection;

        public Dictionary<PlatFormTag, IntendList> _intentlistobj;

        private string lowercase = "abcdefghijklmnopqrstuvwxyz";
        private string numbers = "0123456789";


        public URLService(IOptions<MongoDbDatabaseSettings> mongoDbDatabaseSettings)
        {
            var mongoClient = new MongoClient(
              mongoDbDatabaseSettings.Value.ConnectionString);

            var mongoDatabase = mongoClient.GetDatabase(
                mongoDbDatabaseSettings.Value.DatabaseName);

            _basePlanCollection = mongoDatabase.GetCollection<BasePlan>(
                mongoDbDatabaseSettings.Value.GoogleUserCollectionName);

            _linksCollection = mongoDatabase.GetCollection<Links>(
                mongoDbDatabaseSettings.Value.LinksCollectionName);

            _IntendListCollection = mongoDatabase.GetCollection<IntendList>(
                mongoDbDatabaseSettings.Value.IntendListCollectionName);

            if (_intentlistobj == null)
            {
                _intentlistobj = new Dictionary<PlatFormTag, IntendList>();
                var allRawData = GetAllIntendList();
                if (allRawData != null && allRawData.Count != 0)
                {
                    foreach (var item in allRawData)
                    {
                        if (item.id.Equals("Youtube"))
                        {
                            if(!_intentlistobj.ContainsKey(PlatFormTag.Youtube))
                            {
                                _intentlistobj.Add(PlatFormTag.Youtube, item);
                            }
                        }
                        if (item.id.Equals("Instagram"))
                        {
                            if (!_intentlistobj.ContainsKey(PlatFormTag.Instagram))
                            {
                                _intentlistobj.Add(PlatFormTag.Instagram, item);
                            }
                        }

                    }

                }
            }

        }

        public Links findURL(string shortid)
        {
            var result = new Links();
            try
            {
                result = _linksCollection.Find(g => g._id.Equals(shortid)).FirstOrDefault<Links>();
            }
            catch (Exception ex)
            {


            }
            return result;
        }

        public string Generate_shortrandomid_url()
        {

            string result = string.Empty;
            try
            {
            step1:
                string nanoid = Nanoid.Nanoid.Generate(lowercase + numbers, 9);
                var userDocRef = findURL(nanoid);
                if (userDocRef != null)
                {
                    if (userDocRef._id != null)
                    {
                        goto step1;
                    }
                }
                else
                {
                    result = nanoid;
                }

            }
            catch (Exception ex)
            {

            }
            return result;


        }

        public async Task UpdateURLHit_count_increment(string shortid)
        {
            var result = new Links();
            try
            {
                var rawobject = _linksCollection.Find(g => g._id.Equals(shortid)).FirstOrDefault<Links>();
                if (rawobject != null && rawobject._id != null)
                {
                    rawobject.click_count = rawobject.click_count + 1;
                }
                await _linksCollection.ReplaceOneAsync(s => s._id == shortid, rawobject);
            }
            catch (Exception ex)
            {


            }
        }

        public async Task<Links> CreateURL(string originalURL, string tag, string shortid, string userid)
        {
            var result = new Links();
            try
            {
                var newObject = new Links(originalURL, tag, shortid, userid);
                var RawData = _linksCollection.InsertOneAsync(newObject);
                RawData.Wait();
                result = findURL(shortid);
            }
            catch (Exception ex)
            {


            }
            return result;
        }

        public async Task<GotoRes> GetSmartURL(GotoReq req, HttpRequest reqHeaders)
        {
            GotoRes result = new GotoRes();
            var Headers = reqHeaders;
            try
            {
                string shortid = req.shortid;
                string tag = req.tag;
                string originalURL = string.Empty;
                string created_at_ = string.Empty;
                string ErrorMsg_ = string.Empty;

                string mobile_os = string.Empty;
                string devicetype = string.Empty;
                //First check that shortid is present in cache or not
                var PlatFormTag_ = helper.getidentify_platformTag(tag);
                //
                var obj_provided = intend.get_PlatFormintend(_intentlistobj, PlatFormTag_, mobile_os, devicetype, originalURL);
                //console.log("obj_provided :", obj_provided);
                string app_intend = obj_provided.app_intend;
                string os_type = obj_provided.os_type;
                //await urls.url_count_increment(shortid);
                //const obj = { app_intend, os_type, originalURL: url_obj.originalURL };
                //console.log("obj:" + obj.originalURL);
                result = new GotoRes(app_intend, os_type, originalURL, created_at_, false, ErrorMsg_);

            }
            catch (Exception ex)
            {


            }
            return result;

        }


        public void SaveIntendList()
        {
            try
            {
                List<IntendList> intendLists = new List<IntendList>();

                var intendlistobj_yt = new IntendList();
                // For YouTube
                intendlistobj_yt.id = "Youtube";
                intendlistobj_yt.Popular = string.Empty;
                intendlistobj_yt.Social = string.Empty;
                intendlistobj_yt.INTEND_ANDROID = "intent://www.youtube.com/#Intent;package=com.google.android.youtube;scheme=https;end";
                intendlistobj_yt.INTEND_IOS = "vnd.youtube://www.youtube.com/";
                intendlistobj_yt.intend_android_after = "/#Intent;package=com.google.android.youtube;scheme=https;end";
                intendlistobj_yt.intend_android_before = "intent://";
                intendlistobj_yt.intend_ios_after = "/";
                intendlistobj_yt.intend_ios_after = "vnd.youtube://";
                intendLists.Add(intendlistobj_yt);
                // For YouTube
                var intendlistobj_ins = new IntendList();
                intendlistobj_ins.id = "Instagram";
                intendlistobj_ins.Popular = string.Empty;
                intendlistobj_ins.Social = string.Empty;
                intendlistobj_ins.INTEND_ANDROID = "intent://www.instagram.com/xyz#Intent;package=com.instagram.android;scheme=https;end";
                intendlistobj_ins.INTEND_IOS = "instagram://user username=xyz ";
                intendlistobj_ins.intend_android_after = "#Intent;package=com.instagram.android;scheme=https;end";
                intendlistobj_ins.intend_android_before = "intent://";
                intendlistobj_ins.intend_ios_after = "/";
                intendlistobj_ins.intend_ios_after = "instagram://user username=";
                intendLists.Add(intendlistobj_ins);
                _IntendListCollection.InsertMany(intendLists);

            }
            catch (Exception ex)
            {


            }

        }

        public List<IntendList> GetAllIntendList()
        {
            return _IntendListCollection.Find(_ => true).ToList();
        }

        public Dictionary<PlatFormTag, IntendList> GetDicIntendList()
        {
            return this._intentlistobj;
        }
    }
}
