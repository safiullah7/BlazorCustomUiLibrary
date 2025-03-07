using Microsoft.AspNetCore.Components;
using System.Threading.Tasks;
using Microsoft.JSInterop;

namespace Intrigma.UI.Shared.Components.Base;

public partial class DropdownList<T> : IAsyncDisposable
{
    [Inject] public IJSRuntime JsRuntime { get; set; } = default!;
    
    [Parameter] public string Label { get; set; } = string.Empty;
    [Parameter] public List<T> DropdownItems { get; set; } = new();
    [Parameter] public T Value { get; set; } = default!;
    [Parameter] public RenderFragment<T> SelectedValueFragment { get; set; } = default!;
    [Parameter] public EventCallback<T> ValueChanged { get; set; }
    [Parameter] public string ActivatorPlaceholderText { get; set; } = "Select a value";
    [Parameter] public RenderFragment<T>? ItemFragment { get; set; }
    [Parameter] public bool ShowSearch { get; set; }
    [Parameter] public bool RoundedCorners { get; set; }
    [Parameter] public bool ShowBoxShadowOnBody { get; set; }
    [Parameter] public string ValuePropertyName { get; set; }

    private bool isOpen = false;
    private bool _isClosing = false;
    private DotNetObjectReference<DropdownList<T>>? _dotNetHelper;
    private ElementReference DropdownRef;
    private string _searchStr;
    private List<T> _filteredItems;
    private string _cssClassesActivator;
    private string _cssClassesBody;
    private bool _roundedCorners;

    protected override void OnParametersSet()
    {
        if (_roundedCorners != RoundedCorners)
        {
            _roundedCorners = RoundedCorners;
            if (_roundedCorners)
            {
                _cssClassesActivator += " rounded-corners";
                _cssClassesBody += " rounded-corners";
            }
            else
            {
                _cssClassesActivator = _cssClassesActivator.Replace("rounded-corners", "");
                _cssClassesBody = _cssClassesBody.Replace("rounded-corners", "");
            }
        }
        
        base.OnParametersSet();
    }
    
    protected override void OnInitialized()
    {
        _filteredItems = new List<T>(DropdownItems);
        _roundedCorners = RoundedCorners;
        if (RoundedCorners)
        {
            _cssClassesActivator += " rounded-corners";
            _cssClassesBody += " rounded-corners";
        }

        if (ShowBoxShadowOnBody)
        {
            _cssClassesBody += " shadow";
        }
    }

    private void OnSearchInputChanged(string value)
    {
        _searchStr = value;
        FilterItems();
    }

    private void FilterItems()
    {
        if (string.IsNullOrWhiteSpace(_searchStr))
        {
            _filteredItems = new List<T>(DropdownItems);
        }
        else
        {
            if (string.IsNullOrWhiteSpace(ValuePropertyName))
            {
                _filteredItems = DropdownItems
                    .Where(item => item?.ToString()?.Contains(_searchStr, StringComparison.OrdinalIgnoreCase) ?? false)
                    .ToList();
            }
            else
            {
                _filteredItems = DropdownItems
                    .Where(item => item is not null && MatchesFilter(item))
                    .ToList();
            }
        }
        StateHasChanged();
    }

    private bool MatchesFilter(T item)
    {
        var property = typeof(T).GetProperty(ValuePropertyName);
        if (property != null)
        {
            var value = property.GetValue(item)?.ToString();
            return value?.Contains(_searchStr, StringComparison.OrdinalIgnoreCase) ?? false;
        }
        return false;
    }
    
    protected override async Task OnAfterRenderAsync(bool firstRender)
    {
        if (firstRender)
        {
            _dotNetHelper = DotNetObjectReference.Create(this);
        }

        if (isOpen)
        {
            await JsRuntime.InvokeVoidAsync("dropdownHelper.registerClickOutside", DropdownRef, _dotNetHelper);
        }
    }

    [JSInvokable]
    public async Task CloseDropdown()
    {
        if (isOpen)
        {
            await ToggleDropdown();
        }
    }

    private async Task ToggleDropdown()
    {
        if (isOpen)
        {
            _isClosing = true;
            StateHasChanged();
            await Task.Delay(300);
            _isClosing = false;
            isOpen = false;
        }
        else
        {
            isOpen = true;
        }
        
        StateHasChanged();
    }

    private async Task SelectItem(T item)
    {
        Value = item;
        await ValueChanged.InvokeAsync(Value);
        await ToggleDropdown();
    }

    public async ValueTask DisposeAsync()
    {
        if (_dotNetHelper is not null)
        {
            _dotNetHelper.Dispose();
        }
    }
}
