using Business.Abstract;
using Business.Constants;
using Core.Results;
using Core.Tools;
using DataAccess.Abstract;
using Entities.Concrete;
using Entities.Concrete.Dtos;
using System;
using System.Collections.Generic;
using System.Text;

namespace Business.Concrete
{
    public class UserManager : IUserService
    {
        IUserDao _userDao;
        public UserManager(IUserDao userDao)
        {
            _userDao = userDao;
        }

        public IResult Add(User user)
        {
            var currentUser = _userDao.Get(x => x.Email == user.Email);
            if(currentUser != null)
            {
                throw new Exception("Bu E-Posta adresi zaten kayıtlı");
            }
            user.Password = Crypto.MD5Crypt(user.Password);
            _userDao.Add(user);
            return new SuccessResult(Messages.UserAdded);
        }

        public IResult ChangePassword(int userId, ChangePasswordDto changePasswordDto)
        {
            string oldpassword = changePasswordDto.OldPassword;
            string newpassword = changePasswordDto.NewPassword;
            User user = _userDao.Get(x => x.UserId == userId);
            if(user != null)
            {
                if(user.Password == Crypto.MD5Crypt(oldpassword))
                {
                    user.Password = Crypto.MD5Crypt(newpassword);
                    _userDao.Update(user);
                    return new SuccessResult("Şifre başarıyla değiştirildi");
                }
                return new ErrorResult("Şu anki şifreniz doğru değil");
            }
            return new ErrorResult("Bir hata oluştu");
        }

        public IResult Delete(User user)
        {
            _userDao.Delete(user);
            return new SuccessResult(Messages.UserDeleted);
        }

        public IDataResult<List<User>> GetAll()
        {
            var users = _userDao.GetAll();
            users.ForEach(x => x.Password = null);
            return new SuccessDataResult<List<User>>(users);
        }

        public IDataResult<User> GetByEmail(string email)
        {
            return new SuccessDataResult<User>(_userDao.Get(x => x.Email == email));
        }

        public IDataResult<User> GetByEmailAndPassword(string email,string password)
        {
            return new SuccessDataResult<User?>(_userDao.Get(x => x.Email == email && x.Password == Crypto.MD5Crypt(password)));
        }
        
        public IDataResult<User> GetByUserId(int userId)
        {
            var user = _userDao.Get(x => x.UserId == userId);
            return new SuccessDataResult<User>(user);
        }

        public IResult Register(User user)
        {
            var currentUser = _userDao.Get(x => x.Email == user.Email);
            if (currentUser != null)
            {
                throw new Exception("Bu E-Posta adresi zaten kayıtlı");
            }
            user.IsVerified = false;
            user.UserId = default;
            user.Password = Crypto.MD5Crypt(user.Password);
            _userDao.Add(user);
            //_smtpMailService.SendVerificationMail("enescakr01@hotmail.com");
            return new SuccessResult(Messages.UserAdded);
        }

        public IResult Update(User user)
        {
            _userDao.Update(user);
            return new SuccessResult(Messages.UserUpdate);
        }

        public IResult UpdateAccount(User user)
        {
            var currentUser = _userDao.Get(x => x.UserId == user.UserId);
            if(user.Email.ToLower() != currentUser.Email.ToLower())
            {
                var emailUser = _userDao.Get(x => x.Email.ToLower() == user.Email.ToLower());
                if(emailUser != null)
                {
                    return new ErrorResult("E-Posta adresi zaten kayıtlı");
                }
                currentUser.Email = user.Email;
                currentUser.IsVerified = false;
            }
            currentUser.Firstname = user.Firstname;
            currentUser.Lastname = user.Lastname;
            _userDao.Update(currentUser);
            return new SuccessResult("Hesap başarıyla güncellendi");
        }
    }
}
