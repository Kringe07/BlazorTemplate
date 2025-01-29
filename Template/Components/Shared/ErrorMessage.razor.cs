using Microsoft.AspNetCore.Components;

namespace ProjectName.Components.Shared;

public partial class ErrorMessage : ComponentBase
{
    [Parameter] public required string Message { get; set; }
}