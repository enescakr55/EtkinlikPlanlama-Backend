using Business.Abstract;
using Core.Results;
using DataAccess.Abstract;
using Entities.Concrete;
using System;
using System.Collections.Generic;
using System.Text;

namespace Business.Concrete
{
    public class InvitationStatusManager : IInvitationStatusService
    {
        IInvitationStatusesDao _invitationStatusDao;
        public InvitationStatusManager(IInvitationStatusesDao invitationStatusDao)
        {
            _invitationStatusDao = invitationStatusDao;
        }
        public IResult Add(InvitationStatus invitationStatus)
        {
            
            _invitationStatusDao.Add(invitationStatus);
            return new SuccessResult();
        }

        public IResult Delete(InvitationStatus invitationStatus)
        {
            _invitationStatusDao.Delete(invitationStatus);
            return new SuccessResult();
        }

        public IDataResult<List<InvitationStatus>> GetAll()
        {
            return new SuccessDataResult<List<InvitationStatus>>(_invitationStatusDao.GetAll());
        }

        public IDataResult<InvitationStatus> GetById(int invitationStatusId)
        {
            return new SuccessDataResult<InvitationStatus>(_invitationStatusDao.Get(x => x.StatusId == invitationStatusId));
        }

        public IDataResult<InvitationStatus> GetByInvitationId(int invitationId)
        {
            return new SuccessDataResult<InvitationStatus>(_invitationStatusDao.Get(x => x.InvitationId == invitationId));
        }

        public IResult Update(InvitationStatus invitationStatus)
        {
            _invitationStatusDao.Update(invitationStatus);
            return new SuccessResult();
        }
    }
}
