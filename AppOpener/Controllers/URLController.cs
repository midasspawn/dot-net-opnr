using AppOpener.Data.Models;
using AppOpener.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.ComponentModel.DataAnnotations;

namespace AppOpener.Controllers
{

    [ApiController]
    [Authorize]
    public class URLController : ControllerBase
    {
        public IPlatformService platformService;
        public IURLService urlService;
        public IGoogleOAuthService googleOAuthService;
        public URLController(IPlatformService platformService_, IURLService urlService_, IGoogleOAuthService googleOAuthService_)
        {
            platformService = platformService_;
            urlService = urlService_;
            googleOAuthService = googleOAuthService_;
        }


        /// <summary>
        /// Get url from shorten link
        /// </summary>
        /// <param name="req"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("goto/{tag}/{shortid}")]
        //public ActionResult<GotoRes> GetOriginalURLFromshortid([FromRoute] GotoReq req )
        public ActionResult<GotoRes> GetOriginalURLFromshortid([FromRoute] string tag, string shortid, [Required]string devicetype, [Required]string ostype, [Required]string browsertype)
        {
            //string tag= req.tag;
            //string tag= req.tag;
            //string shortid = req.shortid; string devicetype = req.devicetype; string ostype = req.ostype; string browsertype = req.devicetype;
            var reqHeaders = HttpContext.Request;
            var GotoRes = new GotoRes();
            try
            {

                string mobile_os = ostype;
                //check the type of tag
                var tag_type = helper.getidentify_platformTag(tag);
                if (tag_type == null)
                {
                    GotoRes = new GotoRes("", "", "", "", false, "Tag does not exist!");
                }
                var newobject = urlService.findURL(shortid);
                if (newobject != null)
                {
                    if (newobject._id != null)
                    {
                        urlService.UpdateURLHit_count_increment(shortid);
                        var obj_provided = intend.get_PlatFormintend(urlService.GetDicIntendList(), tag_type, mobile_os, devicetype, newobject.originalURL);
                        if (obj_provided != null)
                        {
                            GotoRes = new GotoRes(obj_provided.app_intend, obj_provided.os_type, newobject.originalURL, Convert.ToString(newobject.created_at), false, null);
                        }
                    }
                }



            }
            catch (Exception ex)
            {

            }
            return StatusCode(200, GotoRes);
        }

        /// <summary>
        /// Create short URL for non-loginned Users
        /// </summary>
        /// <param name="req"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("createOpenURL")]
        public ActionResult<CreateOpenURLRes> CreateShortURL([FromBody] CreateOpenURLReq req)
        {
            var reqHeaders = HttpContext.Request;
            var createOpenURLRes_ = new CreateOpenURLRes();
            try
            {
                string tokenID = string.Empty;
                string userid = "123456";

                bool checkURL = platformService.validURL(req.link);
                if (!checkURL)
                {
                    createOpenURLRes_.validateError = "URL is not valid";
                    return StatusCode(400, createOpenURLRes_);
                }
                string tag = validate.get_Tag(req.link);
                string shortid = urlService.Generate_shortrandomid_url();
                var newobject = urlService.CreateURL(req.link, tag, shortid, userid);
                newobject.Wait();
                if (newobject.Result != null && newobject.Result._id != null)
                {
                    createOpenURLRes_ = new CreateOpenURLRes(newobject.Result);
                }
            }
            catch (Exception ex)
            {

            }
            return createOpenURLRes_;
        }
        /// <summary>
        /// Create ShortURL for loginned Users
        /// </summary>
        /// <param name="req"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("createUserURL")]
        public ActionResult<CreateOpenURLRes> CreateUserURL([FromBody] CreateUsersOpenURLReq req)
        {
            var reqHeaders = HttpContext.Request;
            var createOpenURLRes_ = new CreateOpenURLRes();
            try
            {
                string tokenID = string.Empty;
                string userid = "123456";
                // verify token first send by client 
                var tokenInfo=googleOAuthService.GoogleIdTokenVerifier(req.authtoken);
                tokenInfo.Wait();
                if (tokenInfo.Result != null)
                {
                    if (tokenInfo.Result.Subject != null)
                    {
                        userid = tokenInfo.Result.Subject;
                    }
                    else
                    {
                        createOpenURLRes_.validateError = "Invalid Token";
                        return StatusCode(401, createOpenURLRes_);
                    }

                }
                bool checkURL = platformService.validURL(req.link);
                if (!checkURL)
                {
                    createOpenURLRes_.validateError = "URL is not valid";
                    return StatusCode(400, createOpenURLRes_);
                }
                string tag = validate.get_Tag(req.link);
                string shortid = urlService.Generate_shortrandomid_url();
                var newobject = urlService.CreateURL(req.link, tag, shortid, userid);
                newobject.Wait();
                if (newobject.Result != null && newobject.Result._id != null)
                {
                    createOpenURLRes_ = new CreateOpenURLRes(newobject.Result);
                }
            }
            catch (Exception ex)
            {

            }
            return createOpenURLRes_;
        }

    }
}
