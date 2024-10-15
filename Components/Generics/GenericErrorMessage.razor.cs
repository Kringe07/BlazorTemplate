using Microsoft.AspNetCore.Components;

namespace ProjectName.Components.Generics;

public partial class GenericErrorMessage : ComponentBase
{
    [Parameter] public required string Message { get; set; }
}
