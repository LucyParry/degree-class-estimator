using DegreeClassEstimator.Model;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;
using Microsoft.AspNetCore.Components.Rendering;
using System.Globalization;

namespace Web.Components
{
    /// <summary>
    /// 
    /// https://www.meziantou.net/creating-a-inputselect-component-for-enumerations-in-blazor.htm
    /// </summary>
    public class InputSelectEnum<TEnum> : InputBase<TEnum> where TEnum : Enum
    {
        protected override void BuildRenderTree(RenderTreeBuilder builder)
        {
            int sequence = 0;
            builder.OpenElement(sequence, "select");
            builder.AddMultipleAttributes(sequence++, AdditionalAttributes);
            builder.AddAttribute(sequence++, "class", CssClass);
            builder.AddAttribute(sequence++, "value", BindConverter.FormatValue(CurrentValueAsString));
            builder.AddAttribute(sequence++, "onchange", EventCallback.Factory.CreateBinder<string>(this, value => CurrentValueAsString = value, CurrentValueAsString, null));

            foreach (TEnum value in Enum.GetValues(typeof(TEnum)))
            {
                builder.OpenElement(sequence++, "option");
                builder.AddAttribute(sequence++, "value", value.ToString());
                builder.AddContent(sequence++, value.GetDisplayName());
                builder.CloseElement();
            }
            builder.CloseElement();
        }

        protected override bool TryParseValueFromString(string value, out TEnum result, out string validationErrorMessage)
        {
            if (BindConverter.TryConvertTo(value, CultureInfo.CurrentCulture, out TEnum parsedValue))
            {
                result = parsedValue;
                validationErrorMessage = null;
                return true;
            }

            if (string.IsNullOrEmpty(value))
            {
                var nullableType = Nullable.GetUnderlyingType(typeof(TEnum));
                if (nullableType != null)
                {
                    result = default;
                    validationErrorMessage = null;
                    return true;
                }
            }

            result = default;
            validationErrorMessage = $"The {FieldIdentifier.FieldName} field is not valid.";
            return false;
        }
    }
}
