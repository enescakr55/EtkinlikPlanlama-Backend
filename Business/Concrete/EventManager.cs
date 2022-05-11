using Business.Abstract;
using Business.Concrete.Helpers;
using Core.Results;
using DataAccess.Abstract;
using Entities.Concrete;
using Entities.Concrete.Dtos;
using System;
using System.Collections.Generic;
using System.Text;

namespace Business.Concrete
{
    public class EventManager : IEventService
    {
        IEventsDao _eventsDao;
        IInvitationService _invitationService;
        IUserService _userService;
        public EventManager(IEventsDao eventsDao,IInvitationService invationService,IUserService userService)
        {
            _eventsDao = eventsDao;
            _invitationService = invationService;
            _userService = userService;
        }
        public IResult Add(Event e)
        {
            var user = _userService.GetByUserId(e.EventOwner).Data;
            if(user.IsVerified == false)
            {
                return new ErrorResult("Etkinlik oluşturmak için lütfen E-Posta adresinizi doğrulayın."); 
            }
            if(e.Date < DateTime.Now)
            {
                return new ErrorResult("Lütfen tarihi kontrol ediniz");
            }
            var dtmin = (e.Date.Hour * 60 + e.Date.Minute);
            if(e.EndDate != null)
            {
                var enddatemin = (e.EndDate.Value.Hour * 60 + e.EndDate.Value.Minute);
                if(dtmin >= enddatemin)
                {
                    return new ErrorResult("Başlangıç saati bitiş saatine eşit veya bitiş saatinden büyük olamaz");
                }
                e.EndDate = e.EndDate.Value.ToUniversalTime();
            }
            e.Date = e.Date.ToUniversalTime();
            _eventsDao.Add(e);
            return new SuccessResult("Etkinlik oluşturuldu");
        }

        public IResult Delete(Event e)
        {
            _eventsDao.Delete(e);
            return new SuccessResult("Etkinlik başarıyla silindi");
        }
        public IDataResult<Event> GetEventByInvitationId(string code)
        {
            int eventId = _invitationService.GetByCode(code).Data.EventId;
            return new SuccessDataResult<Event>(_eventsDao.Get(x=>x.EventId == eventId));
        }
        public IDataResult<List<Event>> GetAll()
        {
            return new SuccessDataResult<List<Event>>(_eventsDao.GetAll());
        }

        public IDataResult<Event> GetById(int eventId)
        {
            return new SuccessDataResult<Event>(_eventsDao.Get(x => x.EventId == eventId));
        }

        public IDataResult<List<Event>> GetEventsByUserId(int userId)
        {
            return new SuccessDataResult<List<Event>>(_eventsDao.GetAll(x => x.EventOwner == userId));
        }

        public IDataResult<List<Event>> GetPublicEvents()
        {
            return new SuccessDataResult<List<Event>>(_eventsDao.GetAll(x => x.IsPrivate == false && x.Date.AddHours(1) > DateTime.Now));
        }

        public IResult Invite(SendInvitationDTO invitationDto)
        {
            
            Event ev = _eventsDao.Get(x => x.EventId == invitationDto.InvitationInfo.EventId && x.EventOwner == invitationDto.Inviter);
            if(ev == null)
            {
                return new ErrorResult("Etkinliğe bir katılımcı davet etmek için etkinlik sahibi olmalısınız");
            }
            var inviter = _userService.GetByUserId(invitationDto.Inviter);
            if(inviter.Data.Email == invitationDto.InvitationInfo.Email)
            {
                return new ErrorResult("Kendinizi davet edemezsiniz");
            }
            var invited = _invitationService.GetByUserEmailAndEventId(invitationDto.InvitationInfo.Email, invitationDto.InvitationInfo.EventId);
            if(invited.Data != null)
            {
                return new ErrorResult("Bu kullanıcı zaten davet edilmiş");
            }
            var invitationCode = Tools.CreateRandomCode(30);
            invitationDto.InvitationInfo.InvitationDate = DateTime.Now;
            invitationDto.InvitationInfo.Code = invitationCode;
            _invitationService.Add(invitationDto.InvitationInfo);
            return new SuccessResult("Davet Gönderildi");
        }

        public IResult Update(Event e)
        {
            _eventsDao.Update(e);
            return new SuccessResult();
        }

        public IDataResult<List<Event>> GetEventsBetweenTwoDate(DateTime smallDate, DateTime bigDate)
        {
            List<Event> events = _eventsDao.GetAll(x => x.Date > smallDate && x.Date < bigDate);
            return new SuccessDataResult<List<Event>>(events);
        }
       
    }
}
