using Core.Results;
using DataAccess.Abstract;
using Entities.Concrete;
using Entities.Concrete.Dtos;
using System;
using System.Collections.Generic;
using System.Text;

namespace Business.Abstract
{
    public interface IUserService
    {
        IResult Add(User user);
        IResult Delete(User user);
        IResult Update(User user);
        IDataResult<List<User>> GetAll();
        IDataResult<User> GetByUserId(int userId);
        IDataResult<User> GetByEmailAndPassword(string email,string password);
        IResult Register(User user);
        IDataResult<User> GetByEmail(string email);
        IResult UpdateAccount(User user);
        IResult ChangePassword(int userId,ChangePasswordDto changePasswordDto);
    }
}
