using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;
using ProjectName.CustomAttribute;


namespace ProjectName.Components.Shared
{
    public partial class ImageUploader
    {
        [Parameter] public byte[] Image { get; set; }
        public string FileError { get; set; } = string.Empty;
        public bool FileModelIsValid { get; set; }
        private List<string> _checkType = new List<string>() { ".jpg", ".png" };
        private ImageModel _imageModel = new ImageModel();

        public class ImageModel()
        {
            [RequiredCustom("Foto")]
            [MaxFileSize(10 * 1024 * 1024, ErrorMessage = "File size cannot exceed 10 MB.")]
            public byte[] Image { get; set; } = new byte[0];
        }

        private async Task SingleUpload(InputFileChangeEventArgs e)
        {
            const int MaxBytes = 10_000_000;
            MemoryStream ms = new MemoryStream();
            try
            {
                await e.File.OpenReadStream(MaxBytes).CopyToAsync(ms);
                FileError = "";
            }
            catch (Exception ex)
            {
                FileError = "het bestand dat je wil opleveren is te groot";
                StateHasChanged();
                return;
            }
            string Regex = Path.GetExtension(e.File.Name).ToLowerInvariant();

            if (await CheckFileType(Regex))
            {
                _imageModel.Image = ms.ToArray();
                Image = _imageModel.Image;
            }
            else
            {
                FileError = "Dit is niet het correct file type";
                StateHasChanged();
                return;
            }
        }

        public async Task<bool> CheckFileType(string FileType)
        {
            if (_checkType.Any(c => c.Equals(FileType)))
            {
                return await Task.FromResult(true);
            }
            return await Task.FromResult(false);
        }
    }
}
