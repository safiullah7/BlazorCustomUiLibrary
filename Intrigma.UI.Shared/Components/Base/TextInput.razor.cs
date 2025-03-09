using System.Linq.Expressions;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;

namespace Intrigma.UI.Shared.Components.Base;

public partial class TextInput
{
    [CascadingParameter] private EditContext? EditContext { get; set; }
    
    [Parameter] public string Label { get; set; } = string.Empty;
    [Parameter] public string Value { get; set; }
    [Parameter] public EventCallback<string> ValueChanged { get; set; }
    [Parameter] public string Type { get; set; } = "text";
    [Parameter] public string Placeholder { get; set; }
    [Parameter] public Expression<Func<string>> For { get; set; } 
    [Parameter] public EventCallback<string> OnInputChanged { get; set; }
    [Parameter] public string ColSize { get; set; } = "col-md-12";
    [Parameter] public bool ShowClearIcon { get; set; }
    [Parameter] public bool RoundedCorners { get; set; }

    private string _cssClasses = "form-control ";
    private bool _roundedCorners;
    private bool _isInvalid;
    private string _validCssClasses = string.Empty;
    private string? _value;

    protected override void OnParametersSet()
    {
        if (_value != Value)
        {
            _value = Value;
            StateHasChanged();
        }
        if (_roundedCorners != RoundedCorners)
        {
            _roundedCorners = RoundedCorners;
            if (_roundedCorners)
            {
                _cssClasses += " rounded-corners";
            }
            else
            {
                _cssClasses = _cssClasses.Replace("rounded-corners", "");
            }
        }
    }

    protected override void OnInitialized()
    {
        _value = Value;
        _roundedCorners = RoundedCorners;
        if (_roundedCorners)
        {
            _cssClasses += " rounded-corners";
        }
    }

    private async Task OnInput(ChangeEventArgs arg)
    {
        _value = arg.Value?.ToString();
        Value = _value;
        await ValueChanged.InvokeAsync(_value);
        await OnInputChanged.InvokeAsync(_value);
    }

    private void ClearText()
    {
        _value = string.Empty;
        Value = _value;
        StateHasChanged();
    }

    private void OnBlur()
    {
        if (EditContext != null && For != null)
        {
            var fieldIdentifier = FieldIdentifier.Create(For);
            EditContext.NotifyFieldChanged(fieldIdentifier);

            _isInvalid = EditContext.GetValidationMessages(fieldIdentifier).Any();
        
            _validCssClasses = _isInvalid ? " invalid" :
                _validCssClasses.Contains("invalid") ? _validCssClasses.Replace("invalid", "") : "";
            
            _cssClasses = _cssClasses.Replace("invalid", "");
            _cssClasses = _cssClasses.Replace("valid", "");
            
            _cssClasses += $" {_validCssClasses}";
        }
    }
}