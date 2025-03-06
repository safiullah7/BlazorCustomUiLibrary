using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;

namespace Intrigma.UI.Shared.Components.Base;

public partial class Button
{
    [Parameter] public string Type { get; set; }
    [Parameter] public string Styles { get; set; }
    [Parameter] public bool RoundedCorners { get; set; }
    [Parameter] public ButtonType ButtonType { get; set; } = ButtonType.Primary;
    [Parameter] public ButtonSize Size { get; set; } = ButtonSize.Medium;
    [Parameter] public RenderFragment ChildContent { get; set; }
    [Parameter] public EventCallback<MouseEventArgs> OnClickCb { get; set; }

    private string _cssClasses = "btn custom-button";

    protected override void OnInitialized()
    {
        switch (ButtonType)
        {
            case ButtonType.Primary:
                _cssClasses += " btn-primary";
                break;
            case ButtonType.Secondary:
                _cssClasses += " btn-secondary";
                break;
            case ButtonType.Danger:
                _cssClasses += " btn-danger";
                break;
            case ButtonType.Warning:
                _cssClasses += " btn-warning";
                break;
            case ButtonType.Success:
                _cssClasses += " btn-success";
                break;
        }

        if (RoundedCorners)
            _cssClasses += " button-rounded-corner";
        
        switch (Size)
        {
            case ButtonSize.Small:
                _cssClasses += " button-sm";
                break;
            case ButtonSize.Medium:
                _cssClasses += " button-md";
                break;
            case ButtonSize.Large:
                _cssClasses += " button-lg";
                break;
        }
        
        base.OnInitialized();
    }

    private async Task OnClick()
    {
        await OnClickCb.InvokeAsync();
    }
}

public enum ButtonType
{
    Primary,
    Secondary,
    Success,
    Warning,
    Danger
}

public enum ButtonSize
{
    Small,
    Medium,
    Large
}