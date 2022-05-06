using Business.Abstract;
using Core.Results;
using Entities.Concrete;
using Entities.Concrete.Dtos;
using Microsoft.AspNetCore.Authorization;
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
    public class UsersController : ControllerBase
    {
        IUserService _userService;
        ISMTPMailService _smtpMailService;
        IAccountValidationCodeService _accountValidationCodeService;
        public UsersController(IUserService userService, ISMTPMailService smtpMailService, IAccountValidationCodeService accountValidationCodeService)
        {
            _userService = userService;
            _smtpMailService = smtpMailService;
            _accountValidationCodeService = accountValidationCodeService;
        }
        [Authorize(Roles = "Admin")]
        [HttpPost("add")]
        public IActionResult Add(User user)
        {
            return Ok(_userService.Add(user));
        }
        [HttpPost("register")]
        public IActionResult Register(User user)
        {
            var reg = _userService.Register(user);
            if (reg.Success)
            {
                _smtpMailService.SendVerificationMail(user.Email);
            }
            return Ok(reg);
        }
        [Authorize(Roles = "Admin")]
        [HttpPost("delete")]
        public IActionResult Delete(User user)
        {
            return Ok(_userService.Delete(user));
        }
        [Authorize(Roles ="Admin")]
        [HttpPost("update")]
        public IActionResult Update(User user)
        {
            return Ok(_userService.Update(user));
        }
        [HttpPost("updateaccount")]
        public IActionResult UpdateAccount(User user)
        {
            var userid = int.Parse(HttpContext.User.Claims.First(i => i.Type == ClaimTypes.Name).Value);
            user.UserId = userid;
            return Ok(_userService.UpdateAccount(user));
        }
        [Authorize(Roles = "Admin")]
        [HttpGet("getall")]
        public IActionResult GetAll()
        {
            return Ok(_userService.GetAll());
        }
        [HttpPost("changepassword")]
        public IActionResult ChangePassword(ChangePasswordDto changePasswordDto)
        {
            var userid = int.Parse(HttpContext.User.Claims.First(i => i.Type == ClaimTypes.Name).Value);
            return Ok(_userService.ChangePassword(userid,changePasswordDto));
        }
        [Authorize(Roles = "Admin")]
        [HttpGet("getbyid")]
        public IActionResult GetByUserId(int id)
        {
            return Ok(_userService.GetByUserId(id));
        }
        [HttpGet("getme")]
        public IActionResult GetMe()
        {
            var userid = int.Parse(HttpContext.User.Claims.First(i => i.Type == ClaimTypes.Name).Value);
            var user = _userService.GetByUserId(userid);
            user.Data.Password = null;
            return Ok(user);
        }
        [HttpGet("verify")]
        public IActionResult Verify(string code)
        {
            return Ok(_accountValidationCodeService.ValidateAccount(code));
        }
        [HttpGet("resendmail")]
        public IActionResult ResendMail()
        {
            var email = HttpContext.User.Claims.First(i => i.Type == ClaimTypes.NameIdentifier).Value;
            return Ok(_smtpMailService.SendVerificationMail(email));
        }
    }
}
