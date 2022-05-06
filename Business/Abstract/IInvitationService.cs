using Core.Results;
using Entities.Concrete;
using Entities.Concrete.Dtos;
using System;
using System.Collections.Generic;
using System.Text;

namespace Business.Abstract
{
    public interface IInvitationService
    {
        IResult Add(Invitation invitation);
        IResult Delete(Invitation invitation);
        IResult Update(Invitation invitation);
        IDataResult<List<Invitation>> GetAll();
        IDataResult<Invitation> GetById(int invitationId);
        IResult AcceptInvitation(string code,string message);
        IResult RejectInvitation(string code,string message);
        IDataResult<Invitation> GetByUserEmailAndEventId(string email,int eventid);
        IDataResult<Invitation> GetByCode(string code);
        IDataResult<List<InvitationInfoDto>> GetInvitationInfosByEventId(int eventId);
        IDataResult<List<EventInvitationDto>> GetMyInvitations(string userEmail);
        
    }
}
