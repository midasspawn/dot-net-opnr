using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel.DataAnnotations;

namespace AppOpener.Data.Models
{
    public class URL
    {
        public URL()
        {

        }

         

       
    }

    public class GotoReq
    {
        public GotoReq()
        {

        }

        [Required]
        public string tag { get; set; }
        [Required]
        public string shortid { get; set; }
        [Required]
        public string devicetype { get; set; }
        [Required]
        public string ostype { get; set; }
        [Required]
        public string browsertype { get; set; }
    }

    public class GotoRes
    {
        public GotoRes()
        {

        }

        public GotoRes(string app_intend_,string os_type_,string originalURL_,string created_at_, bool cache_link_,string ErrorMsg_)
        {
            this.app_intend = app_intend_;
            this.os_type = os_type_;
            this.originalURL = originalURL_;
            this.created_at = created_at_;
            this.cache_link = cache_link_;
            this.validateError = ErrorMsg_;
        }



        public string app_intend { get; set; }
        public string os_type { get; set; }
        public string originalURL { get; set; }

        public bool cache_link { get; set; }
        public string created_at { get; set; }

        public string validateError { get; set; }
       
    }

    public class CreateUsersOpenURLReq : CreateOpenURLReq
    {
        public CreateUsersOpenURLReq()
        {

        }

        [Required]
        public string authtoken { get; set; }
    }
    public class CreateOpenURLReq
    {
        public CreateOpenURLReq()
        {

        }

        [Required]
        public string link { get; set; }
        [Required]
        public string apptype { get; set; }
    }

    public class CreateOpenURLRes
    {
        public CreateOpenURLRes()
        {

        }
        public CreateOpenURLRes(string originalURL_,string shortid_,string tag_, string validateError_)
        {
            this.originalURL = originalURL_;
            this.shortid = shortid_;
            this.tag = tag_;
            this.validateError = validateError_;
        }

        public CreateOpenURLRes(Links links_)
        {
            this.originalURL = links_.originalURL;
            this.shortid = links_._id;
            this.tag = links_.tag;
        }

        public string originalURL { get; set; }

        public string shortid { get; set; }

        public string tag { get; set; }

        public string validateError { get; set; }
    }

    public class CreateUserURL
    {
        public CreateUserURL()
        {

        }

        [Required]
        public string link { get; set; }
        [Required]
        public string apptype { get; set; }

        [Required]
        public string authtoken { get; set; }
    }
}
