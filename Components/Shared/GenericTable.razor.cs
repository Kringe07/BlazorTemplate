using System.Drawing;
using Microsoft.AspNetCore.Components;
using ProjectName.DataAccess.Entities;
using ProjectName.DataAccess.Enums;

namespace ProjectName.Components.Shared;

public partial class GenericTable<T> : ComponentBase
{
    [Parameter] public required string Title { get; set; }
    [Parameter] public required List<T> Items { get; set; }
    [Parameter] public required List<ColumnDefinition> Columns { get; set; } = [];
    [Parameter] public Func<(T, string), bool>? SearchFilter { get; set; }
    [Parameter] public Action? OnAdd { get; set; }
    [Parameter] public bool DisableOnAdd { get; set; }
    [Parameter] public Action<T>? OnClick { get; set; }
    [Parameter] public string ErrorMessage { get; set; } = string.Empty;
    [Parameter] public string SearchTerm { get; set; } = string.Empty;
    [Parameter] public RoleType OnAdRoleType { get; set; } = RoleType.Admin;
    private string PageError { get; set; } = string.Empty;
    private bool ShowDisabled { get; set; }

    private List<T> FilteredItems { get; set; } = null!;

    //Initialize component
    protected override void OnInitialized() => FilteredItems = FilterDisabledItems(Items);

    //Filter items
    private void Filter(string? term = null)
    {
        SearchTerm = term ?? SearchTerm;

        if (ShowDisabled)
        {
            FilteredItems = ApplySearchFilter(Items, SearchTerm);
        }
        else
        {
            FilteredItems = ApplySearchFilter(Items, SearchTerm);
            FilteredItems = FilterDisabledItems(FilteredItems);
        }
    }

    //If support for disabling items while toggle these
    private void ToggleShowDisabled()
    {
        ShowDisabled = !ShowDisabled;
        FilteredItems = ShowDisabled ? ApplySearchFilter(Items, SearchTerm) : FilterDisabledItems(FilteredItems);
    }

    
    //if support for disabling items while filter these
    private static List<T> FilterDisabledItems(List<T> items)
    {
        if (items is IEnumerable<IArchivableEntity> archivableEntities)
            return archivableEntities.Where(item => item.IsArchived).Cast<T>().ToList();
        return items;
    }

    //Applies search filter
    private List<T> ApplySearchFilter(IEnumerable<T> items, string searchTerm)
    {
        return SearchFilter == null ? Items : items.Where(x => SearchFilter!((x, searchTerm))).ToList();
    }
}

public abstract class ColumnDefinition
{
    public required string Title { get; init; }
}

public class TextColumn<T> : ColumnDefinition
{
    public required Func<T, string> Data { get; init; }
}

public class ButtonColumn<T> : ColumnDefinition
{
    public Func<T, string>? ButtonName { get; set; }
    public required Func<T, Task> Function { get; set; }
    public Func<T, Color> Color { get; set; } = t => System.Drawing.Color.CornflowerBlue;
}