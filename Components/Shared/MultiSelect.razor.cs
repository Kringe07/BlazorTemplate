using Microsoft.AspNetCore.Components;

namespace ProjectName.Components.Shared;

public partial class MultiSelect<TItem> : ComponentBase
{    [Parameter] public string Label { get; set; } = "Search";
    [Parameter] public string Placeholder { get; set; } = "Type to search...";
    [Parameter] public IEnumerable<TItem> Items { get; set; } = [];
    [Parameter] public EventCallback<List<TItem>> OnSelectionChanged { get; set; }
    [Parameter] public Func<TItem, string> ItemToString { get; set; } = item => item?.ToString() ?? string.Empty;
    [Parameter] public int MaxSelections { get; set; } = int.MaxValue;

    private string SearchText { get; set; } = string.Empty;
    private bool IsDropdownVisible { get; set; }
    private string InputId { get; set; } = Guid.NewGuid().ToString();
    private List<TItem> SelectedItems { get; set; } = new();

    private bool IsMaxReached => SelectedItems.Count >= MaxSelections;

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
        if (IsMaxReached) return;

        SelectedItems.Add(item);
        await OnSelectionChanged.InvokeAsync(SelectedItems);
        SearchText = string.Empty;
        IsDropdownVisible = false;
    }

    private async Task RemoveItem(TItem item)
    {
        SelectedItems.Remove(item);
        await OnSelectionChanged.InvokeAsync(SelectedItems);
    }
}