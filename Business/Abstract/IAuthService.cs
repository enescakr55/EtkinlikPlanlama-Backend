using Core.Results;
using Entities.Concrete.Dtos;
using System;
using System.Collections.Generic;
using System.Text;

namespace Business.Abstract
{
    public interface IAuthService
    {
        IDataResult<ShowLogin> Login(string email, string password);
        IDataResult<ShowLogin> RenewToken(int id);
    }
}
