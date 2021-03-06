using Core.Results;
using Entities.Concrete;
using System;
using System.Collections.Generic;
using System.Text;

namespace Business.Abstract
{
    public interface IJoinEventService
    {
        IResult AddEventJoin(JoinEvent joinEvent);
        IDataResult<List<JoinEvent>> GetEventJoinsByEventId(int eventid);
    }
}
