using Microsoft.AspNetCore.Components;

namespace ProjectName.Components.Shared;

public partial class PageError : ComponentBase
{
    [Parameter]
    public required string Message { get; set; }
}