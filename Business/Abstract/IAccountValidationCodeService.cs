using Core.Results;
using Entities.Concrete;
using System;
using System.Collections.Generic;
using System.Text;

namespace Business.Abstract
{
    public interface IAccountValidationCodeService
    {
        IResult Add(AccountValidationCode accountValidationCode);
        IResult Update(AccountValidationCode accountValidationCode);
        IResult Delete(AccountValidationCode accountValidationCode);
        IDataResult<List<AccountValidationCode>> GetAll();
        IDataResult<AccountValidationCode> GetById(int accountValidationCodeId);
        IDataResult<AccountValidationCode> CreateVerificationCode(int userid);
        IResult ValidateAccount(string code);
    }
}
