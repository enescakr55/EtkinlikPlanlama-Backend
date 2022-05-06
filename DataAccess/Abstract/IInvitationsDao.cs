using Core.Database.Interfaces;
using Entities.Concrete;
using Entities.Concrete.Dtos;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;

namespace DataAccess.Abstract
{
    public interface IInvitationsDao:ICrudBase<Invitation>
    {
        public List<InvitationInfoDto> GetInvitationInfos(Expression<Func<InvitationInfoDto,bool>> filter=null);
        public List<EventInvitationDto> GetEventInvitations(Expression<Func<EventInvitationDto, bool>> filter = null);
    }
}
