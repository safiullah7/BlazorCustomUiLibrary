using System.Linq.Expressions;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;

namespace Intrigma.UI.Shared.Components.Base;

public partial class Checkbox
{
    [CascadingParameter] private EditContext? EditContext { get; set; }
    
    [Parameter] public bool IsChecked { get; set; }
    [Parameter] public string Label { get; set; }
    [Parameter] public string LabelStyles { get; set; }
    [Parameter] public string CheckboxStyles { get; set; }
    [Parameter] public CheckboxSize Size { get; set; } = CheckboxSize.Medium;
    [Parameter] public CheckboxUiType UiType { get; set; } = CheckboxUiType.Primary;
    [Parameter] public Expression<Func<bool>> For { get; set; } 
    [Parameter] public EventCallback<bool> IsCheckedChanged { get; set; }
    [Parameter] public EventCallback<bool> OnCheckedChanged { get; set; }
    
    private string _cssClasses = "cb";
    private string _labelCssClasses = "lb";
    private string _cbId;
    private bool _isInvalid;
    private string _validityCssClass = string.Empty;

    protected override void OnInitialized()
    {
        _cbId = $"cb-{Guid.NewGuid().ToString("N")}";
        switch (Size)
        {
            case CheckboxSize.Small:
                _cssClasses += " cb-sm";
                _labelCssClasses += " lb-sm";
                break;
            case CheckboxSize.Medium:
                _cssClasses += " cb-md";
                _labelCssClasses += " lb-md";
                break;
            case CheckboxSize.Large:
                _cssClasses += " cb-lg";
                _labelCssClasses += " lb-lg";
                break;
            case CheckboxSize.XLarge:
                _cssClasses += " cb-xlg";
                _labelCssClasses += " lb-xlg";
                break;
        }
        switch (UiType)
        {
            case CheckboxUiType.Primary:
                _cssClasses += " cb-primary";
                break;
            case CheckboxUiType.Secondary:
                _cssClasses += " cb-secondary";
                break;
            case CheckboxUiType.Success:
                _cssClasses += " cb-success";
                break;
        }

        base.OnInitialized();
    }

    protected override void OnParametersSet()
    {
        if (EditContext != null && For != null)
        {
            var fieldIdentifier = FieldIdentifier.Create(For);
            _isInvalid = EditContext.GetValidationMessages(fieldIdentifier).Any();

            _validityCssClass = _isInvalid ? " invalid" : 
                _validityCssClass.Contains("invalid") ? _validityCssClass.Replace("invalid", "") : "";
        }
        base.OnParametersSet();
    }

    private async Task CheckboxChanged()
    {
        await OnCheckedChanged.InvokeAsync(IsChecked);
    }
}

public enum CheckboxSize
{
    Small,
    Medium,
    Large,
    XLarge
}

public enum CheckboxUiType
{
    Primary,
    Secondary,
    Success,
}