using Core.Results;
using Entities.Concrete;
using System;
using System.Collections.Generic;
using System.Text;

namespace Business.Abstract
{
    public interface IInvitationStatusService
    {
        IResult Add(InvitationStatus invitationStatus);
        IResult Update(InvitationStatus invitationStatus);
        IResult Delete(InvitationStatus invitationStatus);
        IDataResult<List<InvitationStatus>> GetAll();
        IDataResult<InvitationStatus> GetById(int invitationStatusId);
        IDataResult<InvitationStatus> GetByInvitationId(int invitationId);
    }
}
