using System.Text;
using System.Text.Json;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Components;
using ProjectName.DataAccess.Entities;
using ProjectName.DataAccess.Repository;

namespace ProjectName.Components.Pages;

[Authorize(Roles = "Admin")]
public partial class AuditView : ComponentBase
{
    [Parameter] public required string Id { get; set; }
    [Inject] private AuditRepository AuditRepository { get; set; } = null!;
    private Audit? Audit { get; set; }
    private string PageError { get; set;  } = string.Empty;

    //Initialize component
    protected override async Task OnInitializedAsync()
    {
        try
        {
            Audit = await AuditRepository.GetAudit(Convert.ToInt32(Id));
            PageError = string.Empty;
        }
        catch (Exception e)
        {
            PageError = e.Message;
        }
    }

    //Get changed properties of entry
    private IEnumerable<ChangeEntry> GetChanges()
    {
        using var oldValues = JsonDocument.Parse(Audit!.OriginalValue);

        foreach (var newValue in JsonDocument.Parse(Audit.Changes).RootElement.EnumerateObject())
            yield return new ChangeEntry
            {
                EntityName = newValue.Name,
                OldValue = oldValues.RootElement.GetProperty(newValue.Name).ToString(),
                NewValue = newValue.Value.ToString()
            };
    }

    //Format the text
    private static string FormatJsonText(string jsonString)
    {
        using var doc = JsonDocument.Parse(jsonString, new JsonDocumentOptions { AllowTrailingCommas = true });
        var memoryStream = new MemoryStream();
        using (var utf8JsonWriter = new Utf8JsonWriter(memoryStream, new JsonWriterOptions { Indented = true }))
        {
            doc.WriteTo(utf8JsonWriter);
        }
        return new UTF8Encoding()
            .GetString(memoryStream.ToArray());
    }

    private struct ChangeEntry
    {
        public required string EntityName { get; init; }
        public required string OldValue { get; init; }
        public required string NewValue { get; init; }
    }
}