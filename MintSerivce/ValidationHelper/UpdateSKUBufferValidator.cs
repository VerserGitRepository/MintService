using FluentValidation;
using MintSerivce.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MintSerivce.ValidationHelper
{
    public class UpdateSKUBufferValidator : AbstractValidator<UpdateSKUBufferModel>
    {
        public  UpdateSKUBufferValidator()
        {
            RuleFor(d => d.SKU).Cascade(CascadeMode.StopOnFirstFailure).NotEmpty().WithMessage("{PropertyName} is Empty").NotNull().WithMessage("{PropertyName} is Null");

            RuleFor(b => b.BufferCount).Cascade(CascadeMode.StopOnFirstFailure).NotEmpty().WithMessage("{PropertyName} is Empty").NotNull().WithMessage("{PropertyName} is Null")
                 .Must(isPostCodeNumber).WithMessage("SKU Buffer Value Must Be Number!").Matches(@"^(?=.*?[1-9])\d+(\.\d+)?$").WithMessage("SKU Buffer Must Have Atleast One Number!");        

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