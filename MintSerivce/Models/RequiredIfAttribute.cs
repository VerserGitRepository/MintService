using System;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Linq.Expressions;
using System.Web.Mvc;

namespace MintSerivce.Models
{
    public class RequiredIfAttribute : ValidationAttribute
    {
        private string PropertyName { get; set; }
        private string ErrorMessage { get; set; }
        private Object DesiredValue { get; set; }

        public RequiredIfAttribute(String propertyName, Object desiredvalue, String errormessage)
        {
            this.PropertyName = propertyName;
            this.DesiredValue = desiredvalue;
            this.ErrorMessage = errormessage;
        }

        protected override ValidationResult IsValid(object value, ValidationContext context)
        {
            Object instance = context.ObjectInstance;
            Type type = instance.GetType();
            Object proprtyvalue = type.GetProperty(PropertyName).GetValue(instance, null);
            if (proprtyvalue.ToString() == DesiredValue.ToString() && value == null)
            {
                return new ValidationResult(ErrorMessage);
            }
            return ValidationResult.Success;
        }
        public System.Collections.Generic.IEnumerable<ModelClientValidationRule> GetClientValidationRules(ModelMetadata metadata, ControllerContext context)
        {
            var rule = new ModelClientValidationRule();
            rule.ErrorMessage = FormatErrorMessage(metadata.GetDisplayName());
            rule.ValidationParameters.Add("string", ErrorMessage);
            rule.ValidationType = "required";
            yield return rule;
        }
    }
}