using Core.Results;
using Entities.Concrete;
using Entities.Concrete.Dtos;
using System;
using System.Collections.Generic;
using System.Text;

namespace Business.Abstract
{
    public interface IEventRepeatService
    {
        IResult Add(EventRepeatDto eventRepeatDto);
        IResult Delete(EventRepeatDto eventRepeatDto);
        IDataResult<List<EventRepeat>> GetByEventId(int eventId);
        IDataResult<EventRepeat> GetByEventRepeatId(int id);

    }
}
