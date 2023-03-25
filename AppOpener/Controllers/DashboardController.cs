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
    public class DashboardController : ControllerBase
    {
        private readonly IGoogleOAuthService _googleOAuthService;

        public DashboardController(IGoogleOAuthService googleOAuthService) {
            this._googleOAuthService = googleOAuthService;
        }

        [Route("userdata")]
        [HttpPost]
        public async Task<List<GoogleUserclass>> GetAllGoogleUsers([FromBody] AuthReq req)
        {
            return await _googleOAuthService.GetAllGoogleUsers();
        }

    }
}
