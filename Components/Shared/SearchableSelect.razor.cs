using Microsoft.AspNetCore.Components;

namespace ProjectName.Components.Shared;

public partial class SearchableSelect<TItem> : ComponentBase
{
    [Parameter] public string Label { get; set; } = "Search";
    [Parameter] public string Placeholder { get; set; } = "Type to search...";
    [Parameter] public IEnumerable<TItem> Items { get; set; } = [];
    [Parameter] public EventCallback<TItem> OnSelected { get; set; }
    [Parameter] public Func<TItem, string> ItemToString { get; set; } = item => item?.ToString() ?? string.Empty;
    [Parameter] public bool Disabled { get; set; } = false;
    [Parameter] public TItem SelectedItem { get; set; }

    private string SearchText { get; set; } = string.Empty;
    private bool IsDropdownVisible { get; set; }
    private string InputId { get; set; } = Guid.NewGuid().ToString();

    protected override void OnInitialized()
    {
        if (SelectedItem != null)
        {
            SearchText = ItemToString(SelectedItem);
        }
    }

    private IEnumerable<TItem> FilteredItems => 
        string.IsNullOrWhiteSpace(SearchText)
            ? Items
            : Items.Where(item => ItemToString(item).Contains(SearchText, StringComparison.OrdinalIgnoreCase));

    private void ShowDropdown()
    {
        IsDropdownVisible = true;
    }

    private async Task HideDropdownWithDelay()
    {
        await Task.Delay(150);
        IsDropdownVisible = false;
    }

    private async Task SelectItem(TItem item)
    {
        await OnSelected.InvokeAsync(item);
        SearchText = ItemToString(item);
        IsDropdownVisible = false;
    }
}