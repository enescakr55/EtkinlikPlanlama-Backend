using Business.Abstract;
using Entities.Concrete;
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
    public class EventRepeatsController : ControllerBase
    {
        IEventRepeatService _eventRepeatService;
        public EventRepeatsController(IEventRepeatService eventRepeatService)
        {
            _eventRepeatService = eventRepeatService;
        }
        [HttpGet("getbyeventid")]
        public IActionResult GetByEventId(int eventId)
        {
            return Ok(_eventRepeatService.GetByEventId(eventId));
        }
        [HttpPost("add")]
        public IActionResult Add(EventRepeat eventRepeat)
        {
            var userid = HttpContext.User.Claims.First(i => i.Type == ClaimTypes.Name).Value;
            EventRepeatDto eventRepeatDto = new EventRepeatDto();
            eventRepeatDto.EventRepeat = eventRepeat;
            eventRepeatDto.UserId = int.Parse(userid);
            return Ok(_eventRepeatService.Add(eventRepeatDto));
        }
    }
}
