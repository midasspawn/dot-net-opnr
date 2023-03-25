using AppOpener.Data.Models;
using AppOpener.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AppOpener.Controllers
{
    [ApiController]
    [Authorize]
    public class USERController : ControllerBase
    {
        private readonly IGoogleOAuthService _googleOAuthService;
        public USERController(IGoogleOAuthService googleOAuthService)
        {
            this._googleOAuthService = googleOAuthService;
        }
        //checkUserExist Create if User already exist or not If not then new account will be created
        [Route("checkUserExist")]
        [HttpPost]
        public async Task<List<GoogleUserclass>> checkUserExist([FromBody] checkGoogleUserReq req)
        {
            return await _googleOAuthService.GetGoogleUsersBycheckUserExist(req.userid, req.name, req.email);
        }
    }
}
