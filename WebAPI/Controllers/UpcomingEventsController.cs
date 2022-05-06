using Business.Abstract;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UpcomingEventsController : ControllerBase
    {
        IUpcomingEventService _upcomingEventService;
        IEventService _eventService;
        ISMTPMailService _smtpMailService;
        public UpcomingEventsController(IUpcomingEventService upcomingEventService, IEventService eventService, ISMTPMailService smtpMailService)
        {
            _upcomingEventService = upcomingEventService;
            _eventService = eventService;
            _smtpMailService = smtpMailService;
        }
        [HttpGet("SendUpcomingEventMail")]
        public IActionResult SendUpcomingEventMail()
        {
            return Ok(_smtpMailService.SendReminder(9));
        }
        [HttpGet("UpdateUpcomingEvents")]
        public IActionResult UpdateUpcomingEvents()
        {
            return Ok(_upcomingEventService.UpdateUpcomingEvents());
        }
        [HttpGet("GetEventsForTomorrow")]
        public IActionResult GetEventsForTomorrow()
        {
            return Ok(_upcomingEventService.GetEventsForTomorrow());
        }
        [HttpGet("GetPublicEventsForThisWeek")]
        public IActionResult GetEventsForThisWeek()
        {
            return Ok(_upcomingEventService.GetPublicEventsForThisWeek());
        }
        [HttpGet("GetPublicEventsForThisMonth")]
        public IActionResult GetEventsForThisMonth()
        {
            return Ok(_upcomingEventService.GetPublicEventsForThisMonth());
        }

    }
}
