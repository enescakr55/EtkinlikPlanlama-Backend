using Business.Abstract;
using Core.Results;
using Entities.Concrete;
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
    public class EventJoinController : ControllerBase
    {
        IJoinEventService _joinEventService;
        IEventService _eventService;
        public EventJoinController(IJoinEventService joinEventService, IEventService eventService)
        {
            _joinEventService = joinEventService;
            _eventService = eventService;
        }
        [HttpPost("add")]
        public IActionResult addEventJoin(JoinEvent joinEvent)
        {
            return Ok(_joinEventService.AddEventJoin(joinEvent));
        }
        [HttpGet("geteventjoinsbyeventid")]
        public IActionResult GetEventJoinsByEventId(int eventid)
        {
            var userid = HttpContext.User.Claims.First(i => i.Type == ClaimTypes.Name).Value;
            var getEvent = _eventService.GetById(eventid);
            if(getEvent.Data.EventOwner != int.Parse(userid))
            {
                return BadRequest(new ErrorDataResult<List<JoinEvent>>("Katılımcıları yalnızca etkinlik sahibi görüntüleyebilir"));
            }
            return Ok(_joinEventService.GetEventJoinsByEventId(eventid));
        }
    }
}
