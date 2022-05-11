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
    public class InvitationManager : IInvitationService
    {
        IInvitationsDao _invitationsDao;
        IInvitationStatusService _invitationStatusService;
        IUserService _userService;
 
        public InvitationManager(IInvitationsDao invitationsDao, IInvitationStatusService invitationStatusService,IUserService userService)
        {
            _invitationsDao = invitationsDao;
            _invitationStatusService = invitationStatusService;
            _userService = userService;
        }

        public IResult AcceptInvitation(string code, string message = null)
        {
            var invitation = _invitationsDao.Get(x => x.Code == code);
            if(invitation == null)
            {
                return new ErrorResult("Davet kodu bulunamadı");
            }
            var invitationStatus = _invitationStatusService.GetByInvitationId(invitation.InvitationId);
            if(invitationStatus.Data != null)
            {
                _invitationStatusService.Delete(invitationStatus.Data);
            }
            InvitationStatus status = new InvitationStatus();
            status.InvitationId = invitation.InvitationId;
            status.Status = 1;
            status.Message = message;
            _invitationStatusService.Add(status);
            return new SuccessResult("Davet kabul edildi");
        }

        public IResult Add(Invitation invitation)
        {
            invitation.InvitationDate = invitation.InvitationDate.ToUniversalTime();
            _invitationsDao.Add(invitation);
            return new SuccessResult();
        }

        public IResult Delete(Invitation invitation)
        {
            _invitationsDao.Delete(invitation);
            return new SuccessResult();
        }

        public IDataResult<List<Invitation>> GetAll()
        {
            return new SuccessDataResult<List<Invitation>>(_invitationsDao.GetAll());
        }

        public IDataResult<Invitation> GetByCode(string code)
        {
            return new SuccessDataResult<Invitation>(_invitationsDao.Get(x => x.Code == code));
        }

        public IDataResult<Invitation> GetById(int invitationId)
        {
            return new SuccessDataResult<Invitation>(_invitationsDao.Get(x => x.InvitationId == invitationId));
        }

        public IDataResult<Invitation> GetByUserEmailAndEventId(string email, int eventid)
        {
            var invitation = _invitationsDao.Get(x => x.Email.ToLower() == email.ToLower() && x.EventId == eventid);
            return new SuccessDataResult<Invitation>(invitation);
        }

        public IDataResult<List<InvitationInfoDto>> GetInvitationInfosByEventId(int eventId)
        {
            var invitationInfos = _invitationsDao.GetInvitationInfos(x => x.EventInvitation.EventId == eventId);
            invitationInfos.ForEach(r => r.EventInvitation.Code = "");

            return new SuccessDataResult<List<InvitationInfoDto>>(invitationInfos);
        }

        public IDataResult<List<EventInvitationDto>> GetMyInvitations(string userEmail)
        {
            var currentUser = _userService.GetByEmail(userEmail);
            if (currentUser.Data.IsVerified == false)
            {
                return new ErrorDataResult<List<EventInvitationDto>>("Davetlerinizi görmek için lütfen E-Posta doğrulaması yapın");
            }
            return new SuccessDataResult<List<EventInvitationDto>>(_invitationsDao.GetEventInvitations(x=>x.InvitedUserEmail == userEmail && x.EventDate > DateTime.Now));
        }

        public IResult RejectInvitation(string code, string message)
        {
            var invitation = _invitationsDao.Get(x => x.Code == code);
            if (invitation == null)
            {
                return new ErrorResult("Davet kodu bulunamadı");
            }
            var invitationStatus = _invitationStatusService.GetByInvitationId(invitation.InvitationId);
            if (invitationStatus.Data != null)
            {
                _invitationStatusService.Delete(invitationStatus.Data);
            }
            InvitationStatus status = new InvitationStatus();
            status.InvitationId = invitation.InvitationId;
            status.Status = 0;
            status.Message = message;
            _invitationStatusService.Add(status);
            return new SuccessResult("Davet reddedildi");
        }

        public IResult Update(Invitation invitation)
        {
            _invitationsDao.Update(invitation);
            return new SuccessResult();
        }
    }
}
