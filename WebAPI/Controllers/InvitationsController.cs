using Business.Abstract;
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
    public class InvitationsController : ControllerBase
    {
        IInvitationService _invitationService;
        IInvitationStatusService _invitationStatusService;
        public InvitationsController(IInvitationService invitationService, IInvitationStatusService invitationStatusService)
        {
            _invitationService = invitationService;
            _invitationStatusService = invitationStatusService;
        }
        [HttpGet("accept")]
        public IActionResult Accept(string code,string message=null)
        {
            return Ok(_invitationService.AcceptInvitation(code,message));
        }
        [HttpGet("reject")]
        public IActionResult Reject(string code,string message = null)
        {
            return Ok(_invitationService.RejectInvitation(code, message));
        }
        [HttpGet("getinvitationinfos")]
        public IActionResult GetInvitationInfos(int eventId)
        {
            return Ok(_invitationService.GetInvitationInfosByEventId(eventId));
        }
        [HttpGet("getmyinvitations")]
        public IActionResult GetMyInvitations()
        {
            var email = HttpContext.User.Claims.First(x => x.Type == ClaimTypes.NameIdentifier).Value;
            return Ok(_invitationService.GetMyInvitations(email));

        }
        [HttpGet("getall")]
        public IActionResult GetAll()
        {
            return Ok(_invitationService.GetAll());
        }
    }
}
