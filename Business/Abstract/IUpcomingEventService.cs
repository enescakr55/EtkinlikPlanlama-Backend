using Core.Results;
using DataAccess.Abstract;
using Entities.Concrete;
using System;
using System.Collections.Generic;
using System.Text;

namespace Business.Abstract
{
    public interface IUpcomingEventService
    {

        IResult UpdateUpcomingEvents();
        IResult SendNext();
        IDataResult<List<Event>> GetEventsForTomorrow();
        IDataResult<List<Event>> GetPublicEventsForThisWeek();
        IDataResult<List<Event>> GetPublicEventsForThisMonth();

















































































































































































































































































































































    }
}
