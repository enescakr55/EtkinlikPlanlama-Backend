using Business.Abstract;
using Core.Results;
using DataAccess.Abstract;
using Entities.Concrete;
using Entities.Concrete.Dtos;
using System;
using System.Collections.Generic;
using System.Text;

namespace Business.Concrete
{
    public class EventRepeatManager : IEventRepeatService
    {
        IEventRepeatDao _eventRepeatDao;
        IEventService _eventService;
        public EventRepeatManager(IEventRepeatDao eventRepeatDao,IEventService eventService)
        {
            _eventRepeatDao = eventRepeatDao;
            _eventService = eventService;
        }
        public IResult Add(EventRepeatDto addEventRepeat)
        {
            var currentEvent = _eventService.GetById(addEventRepeat.EventRepeat.EventId);
            if(currentEvent.Data.EventOwner == addEventRepeat.UserId)
            {
                _eventRepeatDao.Add(addEventRepeat.EventRepeat);
                return new SuccessResult("Tekrar planı eklendi");
            }
            return new ErrorResult("Tekrar planı eklenemedi");
        }

        public IResult Delete(EventRepeatDto eventRepeatDto)
        {
            var currentEvent = _eventService.GetById(eventRepeatDto.EventRepeat.EventId);
            if (currentEvent.Data.EventOwner == eventRepeatDto.UserId)
            {
                _eventRepeatDao.Add(eventRepeatDto.EventRepeat);
                return new SuccessResult("Tekrar planı silindi");
            }
            return new ErrorResult("Tekrar planı silinemedi");
        }

        public IDataResult<List<EventRepeat>> GetByEventId(int eventId)
        {
            return new SuccessDataResult<List<EventRepeat>>(_eventRepeatDao.GetAll(x => x.EventId == eventId));
        }

        public IDataResult<EventRepeat> GetByEventRepeatId(int id)
        {
            return new SuccessDataResult<EventRepeat>(_eventRepeatDao.Get(x => x.EventRepeatId == id));
        }
    }
}
