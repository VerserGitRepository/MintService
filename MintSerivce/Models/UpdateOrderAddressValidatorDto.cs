using FluentValidation;
using System;

namespace MintSerivce.Models
{
    public class UpdateOrderAddressValidatorDto : AbstractValidator<ViewModel>
    {
        public UpdateOrderAddressValidatorDto()
        {
            RuleFor(x => x.FirstName).NotEmpty().WithMessage("First Name is required");
            RuleFor(x => x.Surname).NotEmpty().WithMessage("Surname is required");
            RuleFor(x => x.Salutation).NotEmpty().WithMessage("Satuation is required");
            RuleFor(x => x.ContactNumber).NotEmpty().WithMessage("Contact Number is required");
            RuleFor(x => x.State).NotEmpty().WithMessage("State is required");
            RuleFor(x => x.AddressLine1).NotEmpty().WithMessage("Address line 1 is required");

            RuleFor(x => x.Locality).NotEmpty().WithMessage("Locality is required.");
            RuleFor(x => x.Postcode).NotEmpty().WithMessage("Postal Code is required").Must(ValidateNumber).WithMessage("Postal Code must be a number with minimum 4 digits");
           



        }

        private bool PAssword_Length(string pass_)
        {


            if (pass_.Length < 8)
            {
                return false;
            }
            else
            {
                return true;
            }
        }

        private bool ValidateNumber(int num)
        {
            return num >= 0000 && num <= 9999;
        }
        //private bool CheckNumber(int num)
        //{
        //    int check = 0;
        //    return (int.TryParse(num, out check));

        //}
    }
}