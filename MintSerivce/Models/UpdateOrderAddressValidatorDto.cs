﻿using FluentValidation;
using System;
using System.Linq;

namespace MintSerivce.Models
{
    public class UpdateOrderAddressValidatorDto : AbstractValidator<ViewModel>
    {
        public UpdateOrderAddressValidatorDto()
        {           
            RuleFor(b => b.Postcode).Cascade(CascadeMode.StopOnFirstFailure).NotEmpty().WithMessage("{PropertyName} is Empty")
                 .Must(isPostLength).WithMessage("Postcode Length Should Be Between 3 To 4 Digits!").Must(isPostCodeNumber).WithMessage("Postcode Must Be Number!").Matches(@"^(?=.*?[1-9])\d+(\.\d+)?$").WithMessage("Postcode Must Have Atleast One Number!");

            RuleFor(c => c.AddressLine1).Cascade(CascadeMode.StopOnFirstFailure).NotEmpty().WithMessage("{PropertyName} is Empty").NotNull().WithMessage("{PropertyName} is Null").Length(1, 42)
                .WithMessage("{PropertyName} Length Should Be Between 1 to 40 Character");

            RuleFor(d => d.FirstName).Cascade(CascadeMode.StopOnFirstFailure).NotEmpty().WithMessage("{PropertyName} is Empty").NotNull().WithMessage("{PropertyName} is Null").Length(1, 20)
                .WithMessage("{PropertyName} Length Should Be Between 1 to 20 Character").Matches(@"^[a-zA-Z-' ]*$").WithMessage("{PropertyName} is Invalid Character");

            RuleFor(e => e.ContactNumber).Cascade(CascadeMode.StopOnFirstFailure).NotEmpty().WithMessage("{PropertyName} is Empty")
             .Must(isContactNumbervalidLength).WithMessage("{PropertyName} is Invalid Length").Must(isNumeric).WithMessage("{PropertyName} Length Should Be Between 8 to 11 Numbers");            

            RuleFor(d => d.Locality).Cascade(CascadeMode.StopOnFirstFailure).NotEmpty().WithMessage("{PropertyName} is Empty").Length(1, 40)
               .WithMessage("{PropertyName} is Invalid").Must(BeAValidChar).WithMessage("{PropertyName} is Invalid Character");

            RuleFor(d => d.Surname).Cascade(CascadeMode.StopOnFirstFailure).NotEmpty().WithMessage("{PropertyName} is Empty").NotNull().WithMessage("{PropertyName} is Null").Length(1, 20)
               .WithMessage("{PropertyName} Length Should Be Between 1 to 20").Matches(@"^[a-zA-Z- ']*$").WithMessage("{PropertyName} is Invalid Character");

            RuleFor(d => d.State).Cascade(CascadeMode.StopOnFirstFailure).NotEmpty().WithMessage("{PropertyName} is Empty").NotNull().WithMessage("{PropertyName} is Null").Length(2, 3)
              .WithMessage("{PropertyName} Length Should Be Between 2 to 3 Character").Must(BeAValidChar).WithMessage("{PropertyName} is Invalid Character");
        }
        protected bool BeAValidChar(string strChar)
        {
            strChar = strChar.Replace(" ", "");
            strChar = strChar.Replace("_", "");
            return strChar.All(Char.IsLetter);
        }
        protected bool isNumeric(int? value)
        {
            string isNumricCheck = value.ToString();
            return isNumricCheck.All(Char.IsNumber);
        }
        protected bool isContactNumbervalidLength(int? value)
        {
            string LengthCheck = Convert.ToString(value).ToString();
            if (LengthCheck.Length > 7 && LengthCheck.Length < 11) { return true; }
            return false;
        }
        protected bool isPostLength(string value)
        {
            if (value.Length > 2 && value.Length < 5)
            {
                return true;
            }
            return false;
        }
        protected bool isPostCodeNumber(string value)
        {
            if (value.All(Char.IsNumber))
            {
                return true;
            }
            return false;
        }
    }
}