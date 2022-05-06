using Business.Abstract;
using Entities.Concrete.Dtos;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]

    public class AuthController : ControllerBase
    {
        IAuthService _authService;
        public AuthController(IAuthService authService)
        {
            _authService = authService;
        }

        [HttpPost("login")]
        public IActionResult Login(LoginDto loginDto)
        {
            var loginp = _authService.Login(loginDto.Email, loginDto.Password);
            if (loginp == null)
            {
                return BadRequest("Kullanıcı adı veya Şifre hatalı");
            }
            return Ok(loginp);
        }
        [HttpGet("renewtoken")]
        public IActionResult RenewToken()
        {
            int id = int.Parse(HttpContext.User.Claims.First(i => i.Type == ClaimTypes.Name).Value);
            return Ok(_authService.RenewToken(id));
        }
    }
}
