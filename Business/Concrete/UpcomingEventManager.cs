using Business.Abstract;
using Core.Results;
using DataAccess.Abstract;
using Entities.Concrete;
using System;
using System.Collections.Generic;
using System.Text;

namespace Business.Concrete
{

    public class UpcomingEventManager:IUpcomingEventService
    {
        IUpcomingEventsDao _upcomingEventsDao;
        IEventService _eventService;
        IInvitationService _invitationService;
        IJoinEventService _joinEventService;
        public UpcomingEventManager(IUpcomingEventsDao upcomingEventsDao,IEventService eventService,IJoinEventService joinEventService, IInvitationService invitationService)
        {
            _upcomingEventsDao = upcomingEventsDao;
            _eventService = eventService;
            _joinEventService = joinEventService;

        }
        public IDataResult<List<Event>> GetEventsForTomorrow()
        {
            DateTime tomorrow = DateTime.Now.AddDays(1);
            DateTime date1 = new DateTime(tomorrow.Year, tomorrow.Month, tomorrow.Day, 0, 0, 1);
            DateTime date2 = new DateTime(tomorrow.Year, tomorrow.Month, tomorrow.Day, 23, 59, 59);

            return _eventService.GetEventsBetweenTwoDate(date1, date2);
        }
        public IDataResult<List<Event>> GetPublicEventsForThisWeek()
        {
            DateTime today = DateTime.Now.AddDays(1);
            DateTime After7Days = DateTime.Now.AddDays(7);
            //DateTime date1 = new DateTime(tomorrow.Year, tomorrow.Month, tomorrow.Day, 0, 0, 1);
            DateTime date2 = new DateTime(After7Days.Year, After7Days.Month, After7Days.Day, 23, 59, 59);

            return _eventService.GetEventsBetweenTwoDate(today, date2);
        }
        public IDataResult<List<Event>> GetPublicEventsForThisMonth()
        {
            DateTime today = DateTime.Now.AddDays(1);
            DateTime After30Days = DateTime.Now.AddDays(30);
            //DateTime date1 = new DateTime(tomorrow.Year, tomorrow.Month, tomorrow.Day, 0, 0, 1);
            DateTime date2 = new DateTime(After30Days.Year, After30Days.Month, After30Days.Day, 23, 59, 59);

            return _eventService.GetEventsBetweenTwoDate(today, date2);
        }
        public IResult UpdateUpcomingEvents()
        {
            var events = _upcomingEventsDao.GetAll();
            foreach (var ev in events)
            {
                _upcomingEventsDao.Delete(ev);
            }
            var eventsForTomorrow = GetEventsForTomorrow();
            foreach(var e in eventsForTomorrow.Data)
            {
                var upcomingEvent = new UpcomingEvent { EventDate = e.Date, EventId = e.EventId };
                _upcomingEventsDao.Add(upcomingEvent);
            }
            return new SuccessResult("Yaklaşan etkinlikler güncellendi");
        }
        public IResult SendNext()
        {
            var upcomingEvents = _upcomingEventsDao.GetAll();
            var upcomingEvent = upcomingEvents[0];
            return new SuccessResult();
        }
    }
}
