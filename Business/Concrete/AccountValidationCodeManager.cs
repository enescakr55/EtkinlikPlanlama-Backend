using Business.Abstract;
using Business.Concrete.Helpers;
using Core.Results;
using DataAccess.Abstract;
using DataAccess.Concrete;
using Entities.Concrete;
using System;
using System.Collections.Generic;
using System.Text;

namespace Business.Concrete
{
    public class AccountValidationCodeManager : IAccountValidationCodeService
    {
        IAccountValidationCodesDao _accountValidationCodesDao;
        IUserService _userService;

        public AccountValidationCodeManager(IAccountValidationCodesDao accountValidationCodesDao, IUserService userService)
        {
            _accountValidationCodesDao = accountValidationCodesDao;
            _userService = userService;
        }
        public IResult Add(AccountValidationCode accountValidationCode)
        {
            _accountValidationCodesDao.Add(accountValidationCode);
            return new SuccessResult();
        }

        public IDataResult<AccountValidationCode> CreateVerificationCode(int userid)
        {
            AccountValidationCode acc = new AccountValidationCode();
            acc.UserId = userid;
            acc.ValidationCode = Tools.CreateRandomCode();
            acc.CreatedDate = DateTime.Now;
            acc.IsUsed = false;
            _accountValidationCodesDao.Add(acc);
            return new SuccessDataResult<AccountValidationCode>(acc);
        }

        public IResult Delete(AccountValidationCode accountValidationCode)
        {
            _accountValidationCodesDao.Delete(accountValidationCode);
            return new SuccessResult();
        }

        public IDataResult<List<AccountValidationCode>> GetAll()
        {
            return new SuccessDataResult<List<AccountValidationCode>>(_accountValidationCodesDao.GetAll());
        }

        public IDataResult<AccountValidationCode> GetById(int accountValidationCodeId)
        {
            return new SuccessDataResult<AccountValidationCode>(_accountValidationCodesDao.Get(x => x.ValidationCodeId == accountValidationCodeId));
        }

        public IResult Update(AccountValidationCode accountValidationCode)
        {
            _accountValidationCodesDao.Update(accountValidationCode);
            return new SuccessResult();
        }

        public IResult ValidateAccount(string code)
        {
            var codeInfo =_accountValidationCodesDao.Get(x => x.ValidationCode == code);
            if(codeInfo == null)
            {
                throw new Exception("Hatalı kod girildi");
            }
            else
            {
                var user = _userService.GetByUserId(codeInfo.UserId);
                user.Data.IsVerified = true;
                _userService.Update(user.Data);
            }
            return new SuccessResult("Hesap onaylandı");
        }
    }
}
