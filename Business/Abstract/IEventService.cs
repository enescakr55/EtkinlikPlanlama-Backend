using Core.Results;
using Entities.Concrete;
using Entities.Concrete.Dtos;
using System;
using System.Collections.Generic;
using System.Text;

namespace Business.Abstract
{
    public interface IEventService
    {
        IResult Add(Event e);
        IResult Delete(Event e);
        IResult Update(Event e);
        IDataResult<List<Event>> GetAll();
        IDataResult<Event> GetById(int eventId);
        IResult Invite(SendInvitationDTO invitation);
        IDataResult<List<Event>> GetPublicEvents();
        IDataResult<List<Event>> GetEventsByUserId(int userId);
        IDataResult<Event> GetEventByInvitationId(string code);
        IDataResult<List<Event>> GetEventsBetweenTwoDate(DateTime smallDate,DateTime bigDate);

    }
}
