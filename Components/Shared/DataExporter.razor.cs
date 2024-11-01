using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
using System.Text;

namespace ProjectName.Components.Shared
{
    public partial class DataExporter
    {
        [Parameter] public IEnumerable<object> Data { get; set; } = new List<object>();
        [Parameter] public string FileName { get; set; } = "export.csv";


        private async Task ExportToCsv()
        {
            var csvData = GenerateCsv(Data);
            var base64Data = Convert.ToBase64String(Encoding.UTF8.GetBytes(csvData));
            var url = $"data:text/csv;base64,{base64Data}";
            await JS.InvokeVoidAsync("BlazorDownloadFile", url, FileName);
        }

        private string GenerateCsv(IEnumerable<object> data)
        {
            var csvBuilder = new StringBuilder();
            var properties = data.First().GetType().GetProperties();
            csvBuilder.AppendLine(string.Join(",", properties.Select(p => p.Name)));
            foreach (var item in data)
            {
                var values = properties.Select(p => p.GetValue(item)?.ToString());
                csvBuilder.AppendLine(string.Join(",", values));
            }
            return csvBuilder.ToString();
        }
    }
}
