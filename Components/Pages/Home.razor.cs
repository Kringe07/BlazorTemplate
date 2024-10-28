using Blazored.Modal;
using Blazored.Modal.Services;
using Microsoft.AspNetCore.Components;
using ProjectName.Components.Modal;

namespace ProjectName.Components.Pages
{
    public partial class Home
    {
        [CascadingParameter] public IModalService Modal { get; set; } = default!;
        [Inject] private NavigationManager navigationManager { get; set; } = default!;

        //protected async override Task OnInitializedAsync()
        //{

        //}

        public async Task Button()
        {
            var options = new ModalOptions()
            {
                HideCloseButton = true,
                DisableBackgroundCancel = true
            };
            ModalParameters ModalParams = new();
            string Email = "Grant.Haley96@gmail.com";
            string SecretKey = AuthService.GenerateSecretKey();
            string qrCodeUri = AuthService.GenerateQrCodeUri(Email, SecretKey);
            await AuthService.EnableTwoFactorAuthenticationAsync(Email, SecretKey);
            ModalParams.Add("Email", Email);
            ModalParams.Add("SecretKey", SecretKey);
            ModalParams.Add("QrCodeImage", AuthService.GenerateQrCodeImage(qrCodeUri));
            IModalReference emailInUseModal = Modal.Show<Setup2FaModal>("Twee-factor-authenticatie instellen", ModalParams, options);
            ModalResult result = await emailInUseModal.Result;
            if (result.Confirmed)
            {
                ToastService.ShowSuccess("Twee-factor-authenticatie klaarzetten gelukt");
                navigationManager.NavigateTo("/");
            }
        }
        public async Task Verifia()
        {
            var options = new ModalOptions()
            {
                HideCloseButton = true,
                DisableBackgroundCancel = true
            };
            ModalParameters ModalParams = new();
            ModalParams.Add("Email", "123@gmail.com");
            IModalReference emailInUseModal = Modal.Show<Verify2FaModal>("Twee-factor-authenticatiecode valideren", ModalParams, options);
            ModalResult result = await emailInUseModal.Result;
            if (result.Confirmed)
            {
            }

        }
    }
}
