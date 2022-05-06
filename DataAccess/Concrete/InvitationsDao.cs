using Core.Database.EntityFramework;
using DataAccess.Abstract;
using Entities.Concrete;
using Entities.Concrete.Dtos;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;
using System.Linq;

namespace DataAccess.Concrete
{
    public class InvitationsDao : EFCrudOperations<Invitation, AppDbContext>, IInvitationsDao
    {
        public List<EventInvitationDto> GetEventInvitations(Expression<Func<EventInvitationDto, bool>> filter = null)
        {
            using(AppDbContext dbContext = new AppDbContext())
            {
                var eventInvitations = from e in dbContext.Events
                                       join invs in dbContext.Invitations
                                       on e.EventId equals invs.EventId
                                       join u in dbContext.Users
                                       on invs.Email equals u.Email
                                       select new EventInvitationDto
                                       {
                                           EventId = e.EventId,
                                           EventDescription = e.EventDescription,
                                           EventName = e.EventName,
                                           EventDate = e.Date,
                                           EventAddress = e.EventAddress,
                                           InvitedUserEmail = u.Email,
                                           InvitationCode = invs.Code,
                                           InvitationId = invs.InvitationId
                                       };
                return filter == null ? eventInvitations.ToList() : eventInvitations.Where(filter).ToList();
                                       
            }
        }

        public List<InvitationInfoDto> GetInvitationInfos(Expression<Func<InvitationInfoDto, bool>> filter = null)
        {
            using (AppDbContext dbContext = new AppDbContext())
            {
                var invitationInfos = from i in dbContext.Invitations
                                      join invs in dbContext.InvitationStatuses
                                      on i.InvitationId equals invs.InvitationId into ps
                                      from invs in ps.DefaultIfEmpty()
                                      select new InvitationInfoDto
                                      {
                                          EventInvitationStatus = invs,
                                          EventInvitation = i
                                      };
                return filter == null ? invitationInfos.ToList() : invitationInfos.Where(filter).ToList();
                                      
            }
        }
    }
}
