using System.Linq.Expressions;
using Microsoft.AspNetCore.Components;

namespace Intrigma.UI.Shared.Components.Base;

public partial class TextInput
{
    [Parameter] public string Label { get; set; } = string.Empty;
    [Parameter] public string Value { get; set; }
    [Parameter] public string Type { get; set; } = "text";
    [Parameter] public string Placeholder { get; set; }
    [Parameter] public Expression<Func<string>> For { get; set; } 
    [Parameter] public EventCallback<string> ValueChanged { get; set; }
    [Parameter] public EventCallback<string> OnInputChanged { get; set; }
    [Parameter] public string ColSize { get; set; } = "col-md-12";
    [Parameter] public bool ShowClearIcon { get; set; }
    [Parameter] public bool RoundedCorners { get; set; }

    private string _cssClasses = "form-control ";
    private bool _roundedCorners;

    protected override void OnParametersSet()
    {
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
        _roundedCorners = RoundedCorners;
        if (_roundedCorners)
        {
            _cssClasses += " rounded-corners";
        }
        base.OnInitialized();
    }

    private async Task OnInput(ChangeEventArgs arg)
    {
        Value = arg.Value?.ToString();
        await ValueChanged.InvokeAsync(Value);
        await OnInputChanged.InvokeAsync(Value);
    }
    
    private void ClearText()
    {
        Value = string.Empty;
        ValueChanged.InvokeAsync(Value);
        StateHasChanged();
    }
}