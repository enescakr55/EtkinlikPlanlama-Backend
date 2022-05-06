using Entities.Concrete;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;

namespace Business.Validator
{
    public class UserValidator:AbstractValidator<User>
    {
        public UserValidator()
        {
            RuleFor(x => x.Email).NotNull().EmailAddress().WithMessage("E-Posta adresini kontrol edin.");
            RuleFor(x => x.Password).NotNull().MinimumLength(8).WithMessage("Şifre 8 karakterden kısa olamaz");
            RuleFor(x => x.Firstname).NotNull().Must(OnlyLetter).WithMessage("Adınız boş olamaz ve yalnızca harf içerebilir");
            RuleFor(x => x.Lastname).NotNull().Must(OnlyLetter).WithMessage("Soyadınız boş olamaz ve yalnızca harf içerebilir");
        }
        public bool OnlyLetter(string str = "")
        {
            try
            {
                return Regex.IsMatch(str, @"^[a-zA-ZçöşğüÜĞŞıİÇÖ]+$");
            }
            catch
            {
                return false;
            }

        }
    }
}
