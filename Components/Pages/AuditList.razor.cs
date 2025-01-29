using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Components;
using ProjectName.DataAccess.Entities;
using ProjectName.DataAccess.Repository;

namespace ProjectName.Components.Pages;

[Authorize(Roles = "Admin")]
public partial class AuditList : ComponentBase
{
    [Parameter] public required string Id { get; set; }
    [Inject] private AuditRepository AuditRepository { get; set; } = null!;
    [Inject] private NavigationManager NavigationManager { get; set; } = null!;
    private List<Audit>? Audits { get; set; }
    private string PageError { get; set; } = string.Empty;

    //Initialize component
    protected override async Task OnInitializedAsync()
    {
        try
        {
            Audits = await AuditRepository.GetAudits();
            PageError = string.Empty;
        }
        catch (Exception e)
        {
            PageError = e.Message;
        }
    }
}