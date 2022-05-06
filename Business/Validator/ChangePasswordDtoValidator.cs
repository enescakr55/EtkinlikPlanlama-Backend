using Entities.Concrete.Dtos;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Text;

namespace Business.Validator
{
    public class ChangePasswordDtoValidator:AbstractValidator<ChangePasswordDto>
    {
        public ChangePasswordDtoValidator()
        {
            RuleFor(x => x.NewPassword).MinimumLength(8).WithMessage("Yeni Şifre 8 karakterden kısa olamaz.");
        }
    }
}
