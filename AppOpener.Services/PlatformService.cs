using System;
using System.Collections.Generic;
using System.Text;
using AppOpener.Data.Models;
using Microsoft.AspNetCore.Http;

namespace AppOpener.Services
{
    public interface IPlatformService
    {

        CreateOpenURLRes CreateOpenLink(CreateOpenURLReq req);

        bool validURL(string url_);

    }
    public class PlatformService : IPlatformService
    {

        public PlatformService()
        {
            
        }

        public bool validURL(string url_)
        {
            return validate.validURL(url_);
        }

        public CreateOpenURLRes CreateOpenLink(CreateOpenURLReq req)
        {
            var result = new CreateOpenURLRes();
            try
            {
                string originalURL = req.link;
                string tag = req.apptype;

                //            string user = "1234567890"; //userid for openlinks 
                //            var url_obj = { };
                //            bool checkURL = validate.validURL(originalURL);

                //            // console.log("original URL : " + originalURL);
                //            // console.log("tag:" + tag);
                //            // console.log("checkURL: " + checkURL);
                //            // console.dir("Req baseURL : " + req.baseUrl);

                //            if (!checkURL)
                //            {
                //                validateError = `URL is not valid`;
                //                return res.status(400).send("URL is invalid");
                //            }
                //            //Verify tag and url
                //            tag = get_Tag(originalURL);
                
            //   const shortid = await urls.generate_random_url();
    //            // console.log(shortid);
    //            const url = await urls.createURL(originalURL, tag, shortid, user);
    //            const created_at = await urls.createURL(originalURL, tag, shortid, user);

    //            url_obj.tag = tag;
    //            url_obj.originalURL = originalURL;
    //            url_obj.created_at = created_at

    ////shortid added to cache 
    ////client.setex(shortid, 432000, JSON.stringify(originalURL));
    //            client.setex(shortid, 432000, JSON.stringify(url_obj));

    //            //res.send({ originalURL, url, tag });
    //            res.send({ originalURL, shortid, tag });

    //            //res.send(JSON.stringify(req.body));
            }
            catch (Exception ex)
            {

                
            }
            return result;

        }

    }
}
