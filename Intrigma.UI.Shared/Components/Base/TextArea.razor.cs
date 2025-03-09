using System.Linq.Expressions;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;

namespace Intrigma.UI.Shared.Components.Base;

public partial class TextArea
{
    [CascadingParameter] private EditContext? EditContext { get; set; }
    
    [Parameter] public string Value { get; set; }
    [Parameter] public string Style { get; set; }
    [Parameter] public string Label { get; set; } = string.Empty;
    [Parameter] public string Placeholder { get; set; }
    [Parameter] public int Rows { get; set; } = 5;
    [Parameter] public bool RoundedCorners { get; set; }
    [Parameter] public EventCallback<string> ValueChanged { get; set; }
    [Parameter] public Expression<Func<string>> For { get; set; } 
    
    private string _value;
    private string _cssClasses = "textarea";
    private bool _roundedCorners;
    private bool _isInvalid;
    private string _validCssClasses = string.Empty;

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
        
        base.OnParametersSet();
    }

    protected override void OnInitialized()
    {
        _value = Value;
        _roundedCorners = RoundedCorners;
        if (_roundedCorners)
        {
            _cssClasses += " rounded-corners";
        }
        
        base.OnInitialized();
    }

    private async Task HandleOnChange(ChangeEventArgs args)
    {
        _value = args?.Value?.ToString();
        Value = _value;

        await ValueChanged.InvokeAsync(_value);
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
        }
    }
}