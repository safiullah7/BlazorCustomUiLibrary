using System.ComponentModel.DataAnnotations;

namespace Intrigma.UI.Shared.Validators;

public class MustBeTrueAttribute : ValidationAttribute
{
    public override bool IsValid(object value)
    {
        return value is bool b && b;
    }
}