using System.Drawing;
using Microsoft.AspNetCore.Components;

namespace ProjectName.Components.Shared;

public partial class GenericTable<T> : ComponentBase
{
    [Parameter] public required string Title { get; set; }
    [Parameter] public required List<T> Items { get; set; }
    [Parameter] public required List<ColumnDefinition<T>> Columns { get; set; } = [];
    [Parameter] public Func<(T, string), bool>? SearchFilter { get; set; }
    [Parameter] public Action? OnAdd { get; set; }
    [Parameter] public Action<T>? OnClick { get; set; }
    [Parameter] public string? ErrorMessage { get; set; }
    [Parameter] public string SearchTerm { get; set; } = string.Empty;

    private List<T> FilteredItems { get; set; } = null!;

    protected override void OnInitialized() => FilteredItems = Items;
}

public abstract class ColumnDefinition<T>
{
    public required string Title { get; set; }
}

public class TextColumn<T> : ColumnDefinition<T>
{
    public required Func<T, string> Data { get; set; }
}

public class ButtonColumn<T> : ColumnDefinition<T>
{
    public Func<T, string>? ButtonName { get; set; }
    public required Func<T, Task> Function { get; set; }
    public Func<T, Color> Color { get; set; } = t => System.Drawing.Color.CornflowerBlue;
}