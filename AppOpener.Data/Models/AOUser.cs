using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel.DataAnnotations;

namespace AppOpener.Data.Models
{
    public class AOUser
    {
        public AOUser()
        {

        }
    }

    public class CheckUserExist
    {
        public CheckUserExist()
        {
        }

        [Required]
        public string name { get; set; }

        [Required]
        public string email { get; set; }

        [Required]
        public string userid { get; set; }
    }
}
