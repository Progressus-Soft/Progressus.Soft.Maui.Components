using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Reflection;
using System.Text;

namespace Progressus.Soft.Maui.Components.Validation;

public static class FormValidation
{
    public static List<ValidationResult> Validate(object model, Layout page)
    {
        HideValidationFields(model, page);
        var errors = new List<ValidationResult>();
        var context = new ValidationContext(model);
        bool isValid = Validator.TryValidateObject(model, context, errors, true);
        if (!isValid)
        {
            ShowValidationFields(errors, model, page);
        }
        return errors;
    }
    public static bool IsFormValid(object model, Layout page)
    {
        HideValidationFields(model, page);
        var errors = new List<ValidationResult>();
        var context = new ValidationContext(model);
        bool isValid = Validator.TryValidateObject(model, context, errors, true);
        if (!isValid)
        {
            ShowValidationFields(errors, model, page);
        }
        return errors.Count() == 0;
    }
    private static void HideValidationFields
        (object model, Layout page, string validationLabelSuffix = "Error")
    {
        if (model == null) { return; }
        var properties = GetValidatablePropertyNames(model);
        foreach (var propertyName in properties)
        {
            var errorControlName =
            $"{propertyName.Replace(".", "_")}{validationLabelSuffix}";
            var errorLabels = page.Children.Where(x => x is ErrorLabel);
            if (errorLabels.Any())
            {
                var control = errorLabels.FirstOrDefault(l => (l as ErrorLabel).Name == errorControlName);
                if (control != null)
                {
                    (control as ErrorLabel).IsVisible = false;
                }
            }
        }
    }
    private static void ShowValidationFields
    (List<ValidationResult> errors,
    object model, Layout page, string validationLabelSuffix = "Error")
    {
        if (model == null) { return; }
        foreach (var error in errors)
        {
            var memberName = $"{model.GetType().Name}_{error.MemberNames.FirstOrDefault()}";
            memberName = memberName.Replace(".", "_");
            var errorControlName = $"{memberName}{validationLabelSuffix}";
            var frameErrorControlName = $"{errorControlName}Frame";
            var errorLabels = page.Children.Where(x => x is ErrorLabel);
            if (errorLabels.Any())
            {
                var control = errorLabels.FirstOrDefault(l => (l as ErrorLabel).Name == errorControlName);
                if (control != null)
                {
                    (control as ErrorLabel).Text = $"{error.ErrorMessage}{Environment.NewLine}";
                    (control as ErrorLabel).IsVisible = true;
                }
            }
        }
    }
    private static IEnumerable<string> GetValidatablePropertyNames(object model)
    {
        var validatableProperties = new List<string>();
        var properties = GetValidatableProperties(model);
        foreach (var propertyInfo in properties)
        {
            var errorControlName = $"{propertyInfo.DeclaringType.Name}.{propertyInfo.Name}";
            validatableProperties.Add(errorControlName);
        }
        return validatableProperties;
    }
    private static List<PropertyInfo> GetValidatableProperties(object model)
    {
        var properties = model.GetType().GetProperties().Where(prop => prop.CanRead
            && prop.GetCustomAttributes(typeof(ValidationAttribute), true).Any()
            && prop.GetIndexParameters().Length == 0).ToList();
        return properties;
    }
}
