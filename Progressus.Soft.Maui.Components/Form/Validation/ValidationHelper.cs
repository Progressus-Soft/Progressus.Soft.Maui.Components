using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Reflection;
using System.Text;
using static System.Runtime.InteropServices.JavaScript.JSType;

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

            var imageControlName =
            $"Image{propertyName.Replace(".", "_")}{validationLabelSuffix}";

            //Colored border
            var errorBorderItem = page.Children
                .Where(s => s is StackLayout)
                .SelectMany(s => (s as StackLayout)!.Children)
                .Where(c => c is BorderItem)
                .FirstOrDefault(c => ((c as BorderItem)!.Content as Grid)!.Children.Any(cc => cc is ErrorImage && (cc as ErrorImage).Name == imageControlName));

            if (errorBorderItem != null)
            {
                var errorImage = ((errorBorderItem as BorderItem)?.Content as Grid)?.FirstOrDefault(c => c is ErrorImage && (c as ErrorImage).Name == imageControlName);
                if (errorImage != null && errorImage is ErrorImage) (errorImage as ErrorImage).IsVisible = false;
                (errorBorderItem as BorderItem).Stroke = Color.Parse("Transparent");
            }


            //Error Label
            var errorLabel = page.Children
                .Where(s => s is StackLayout)
                .SelectMany(s => (s as StackLayout).Children)
                .Where(c => c is ErrorLabel)
                .FirstOrDefault(l => (l as ErrorLabel).Name == errorControlName);
            if (errorLabel != null)
            {
                (errorLabel as ErrorLabel).IsVisible = false;
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
            var imageControlName = $"Image{memberName}{validationLabelSuffix}";

            //Colored border
            var errorBorderItem = page.Children
                .Where(s => s is StackLayout)
                .SelectMany(s => (s as StackLayout)!.Children)
                .Where(c => c is BorderItem)
                .FirstOrDefault(c => ((c as BorderItem)!.Content as Grid)!.Children.Any(cc => cc is ErrorImage && (cc as ErrorImage).Name == imageControlName));

            if (errorBorderItem != null)
            {
                var errorImage = ((errorBorderItem as BorderItem)?.Content as Grid)?.FirstOrDefault(c => c is ErrorImage && (c as ErrorImage).Name == imageControlName);
                if (errorImage != null && errorImage is ErrorImage)
                {
                    (errorImage as ErrorImage)!.IsVisible = true;
                    ToolTipProperties.SetText((errorImage as ErrorImage)!, error);
                }
                (errorBorderItem as BorderItem)!.Stroke = Color.Parse("#ff4c4b");
            }

            //Error Label
            var errorLabel = page.Children
                .Where(s => s is StackLayout)
                .SelectMany(s => (s as StackLayout).Children)
                .Where(c => c is ErrorLabel)
                .FirstOrDefault(l => (l as ErrorLabel).Name == errorControlName);
            if (errorLabel != null)
            {
                (errorLabel as ErrorLabel).Text = $"{error.ErrorMessage}{Environment.NewLine}";
                (errorLabel as ErrorLabel).IsVisible = true;
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
