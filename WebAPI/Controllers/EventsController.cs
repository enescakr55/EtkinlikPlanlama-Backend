using Business.Abstract;
using Core.Results;
using DataAccess.Concrete;
using Entities.Concrete;
using Entities.Concrete.Dtos;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EventsController : ControllerBase
    {
        IEventService _eventService;
        ISMTPMailService _smtpMailService;
        IUserService _userService;
        public EventsController(IEventService eventService, ISMTPMailService smtpMailService,IUserService userService)
        {
            _eventService = eventService;
            _smtpMailService = smtpMailService;
            _userService = userService;
        }
        [Authorize]
        [HttpPost("addevent")]
        public IActionResult AddEvent(Event e)
        {
            var userid = HttpContext.User.Claims.First(i => i.Type == ClaimTypes.Name).Value;
            e.EventOwner = int.Parse(userid);
            
            return Ok(_eventService.Add(e));
        }
        [HttpPost("invite")]
        public IActionResult Invite(Invitation invitation)
        {

            var userid = int.Parse(HttpContext.User.Claims.First(i => i.Type == ClaimTypes.Name).Value);
            SendInvitationDTO sendInvitationDTO = new SendInvitationDTO();
            sendInvitationDTO.InvitationInfo = invitation;
            sendInvitationDTO.Inviter = userid;
            var inv = _eventService.Invite(sendInvitationDTO);
            if (inv.Success)
            {
                _smtpMailService.SendInvitation(sendInvitationDTO);
            }
            return Ok(inv);

            
        }
        [HttpGet("public")]
        public IActionResult Public()
        {
           /* using(AppDbContext _context = new AppDbContext())
            {
                return Ok(await _context.Events.Include(x => x.User).FirstOrDefaultAsync(x => x.User.UserId == x.EventOwner));
            } */
           return Ok(_eventService.GetPublicEvents());
            
        }
        [HttpGet("myevents")]
        public IActionResult MyEvents()
        {
            var userid = int.Parse(HttpContext.User.Claims.First(i => i.Type == ClaimTypes.Name).Value);

            return Ok(_eventService.GetEventsByUserId(userid));
        }
        [HttpGet("geteventbyinvitationid")]
        public IActionResult GetEventByInvitationId(string code)
        {
            return Ok(_eventService.GetEventByInvitationId(code));
        }
        [HttpGet("delete")]
        public IActionResult DeleteEvent(int eventid)
        {
            var userid = int.Parse(HttpContext.User.Claims.First(i => i.Type == ClaimTypes.Name).Value);
            var eventx = _eventService.GetById(eventid);
            if(eventx.Data.EventOwner == userid)
            {
                return Ok(_eventService.Delete(eventx.Data));
            }
            else
            {
                return BadRequest(new ErrorResult("Etkinliği silmek için etkinlik sahibi olmalısınız."));
            }
        }
    }
}
