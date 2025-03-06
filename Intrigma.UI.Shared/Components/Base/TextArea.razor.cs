using Microsoft.AspNetCore.Components;

namespace Intrigma.UI.Shared.Components.Base;

public partial class TextArea
{
    [Parameter] public string Value { get; set; }
    [Parameter] public string Style { get; set; }
    [Parameter] public string Placeholder { get; set; }
    [Parameter] public bool RoundedCorners { get; set; }
    [Parameter] public EventCallback<string> ValueChanged { get; set; }
    
    private string _value;
    private string _cssClasses = "textarea";

    protected override void OnParametersSet()
    {
        _value = Value;
        if (RoundedCorners)
        {
            _cssClasses += " rounded-corners";
        }
        
        base.OnParametersSet();
    }

    private async Task HandleOnChange(ChangeEventArgs args)
    {
        _value = args?.Value?.ToString();

        await ValueChanged.InvokeAsync(_value);
    }
}