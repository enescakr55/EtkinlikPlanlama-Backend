using Core.Database.EntityFramework;
using DataAccess.Abstract;
using Entities.Concrete;
using System;
using System.Collections.Generic;
using System.Text;

namespace DataAccess.Concrete
{
    public class JoinEventsDao:EFCrudOperations<JoinEvent,AppDbContext>,IEventJoinsDao
    {
    }
}
