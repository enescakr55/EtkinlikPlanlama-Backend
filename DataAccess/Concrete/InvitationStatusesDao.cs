using Core.Database.EntityFramework;
using Core.Database.Interfaces;
using DataAccess.Abstract;
using Entities.Concrete;
using System;
using System.Collections.Generic;
using System.Text;

namespace DataAccess.Concrete
{
    public class InvitationStatusesDao:EFCrudOperations<InvitationStatus,AppDbContext>,IInvitationStatusesDao
    {
    }
}
