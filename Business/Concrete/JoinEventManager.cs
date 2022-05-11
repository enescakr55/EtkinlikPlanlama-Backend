using Business.Abstract;
using Business.Concrete.Helpers;
using Core.Results;
using DataAccess.Abstract;
using Entities.Concrete;
using System;
using System.Collections.Generic;
using System.Text;

namespace Business.Concrete
{
    
    public class JoinEventManager : IJoinEventService
    {
        IEventService _eventService;
        IEventJoinsDao _eventJoinsDao;
        IInvitationService _invitationService;
        IUserService _userService;
        public JoinEventManager(IEventJoinsDao eventJoinsDao,IEventService eventService,IInvitationService invitationService,IUserService userService)
        {
            _invitationService = invitationService;
            _eventJoinsDao = eventJoinsDao;
            _eventService = eventService;
            _userService = userService;
        }
        public IResult AddEventJoin(JoinEvent joinEvent)
        {
            var e = _eventService.GetById(joinEvent.EventId).Data;
            joinEvent.Email = joinEvent.Email.ToLower();
            if (joinEvent.Email.ToLower().Equals(_userService.GetByUserId(_eventService.GetById(joinEvent.EventId).Data.EventOwner).Data.Email.ToLower()))
            {
                return new ErrorResult("Etkinliği oluşturan kullanıcı katılımcı olarak davet edilemez");
            }
            if(e.IsPrivate == true)
            {
                return new ErrorResult("Katılmaya çalıştığınız etkinlik özel bir etkinliktir. Yalnızca davetli kişiler katılabilir.");
            }
            var invitation = _invitationService.GetByUserEmailAndEventId(joinEvent.Email, joinEvent.EventId).Data;
            if(invitation != null)
            {
                return new ErrorResult("E-Posta adresiniz bu etkinlik için davetli listesinde.");
            }
            if(_eventJoinsDao.Get(x => x.Email == joinEvent.Email && x.EventId == joinEvent.EventId) != null)
            {
                return new ErrorResult("E-Posta adresi bu etkinlik için zaten kayıtlı");
            }
            
            joinEvent.Code = Tools.CreateRandomCode();
            joinEvent.JoinDate = DateTime.Now.ToUniversalTime();
            _eventJoinsDao.Add(joinEvent);
            return new SuccessResult();
        }

        public IDataResult<List<JoinEvent>> GetEventJoinsByEventId(int eventid)
        {
            var eventJoins = _eventJoinsDao.GetAll(x => x.EventId == eventid);
            return new SuccessDataResult<List<JoinEvent>>(eventJoins, "Etkinliğe katılanlar listelendi");
        }
    }
}
