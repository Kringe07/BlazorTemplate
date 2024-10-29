using Blazored.Modal;
using Blazored.Modal.Services;
using Microsoft.AspNetCore.Components;
using ProjectName.Components.Modal;
using ProjectName.CustomAttribute;
using ProjectName.DataAccess.Entities;
using ProjectName.DataAccess.Repository;
using System.ComponentModel.DataAnnotations;
using BC = BCrypt.Net.BCrypt;

namespace ProjectName.Components.Pages
{
    public partial class Registration
    {
        [CascadingParameter] public IModalService Modal { get; set; } = default!;
        [Inject] private NavigationManager navigationManager { get; set; } = default!;
        [Inject] private AuthenticationService AuthService { get; set; } = default!;
        [Inject] private UserRepository UserRepository { get; set; } = default!;
        public Admin NewAdmin { get; set; } = new Admin();
        public AuthModel authModel = new();
        private string ErrorMessage { get; set; } = string.Empty;

        // This is kinda the registration system
        // 1. check if user email not already exist in the database
        // 2. set every thing in the new object class and save the user in the database
        // 3. show the 2 Fa and set it on
        // 4. show Toast when done and succeeded Return to main page
        private async Task Register()
        {
            User? UserAccount = await UserRepository.GetUserDataAsync(authModel.Email);
            if (UserAccount == null)
            {
                NewAdmin.Password = BC.EnhancedHashPassword(authModel.Password);
                NewAdmin.Email = authModel.Email;
                NewAdmin.SecretKey = "";
                await UserRepository.AddUser(NewAdmin);
                if (await UserRepository.GetUserDataAsync(NewAdmin.Email) is not null)
                {
                    var options = new ModalOptions()
                    {
                        HideCloseButton = true,
                        DisableBackgroundCancel = true
                    };
                    ModalParameters ModalParams = new();
                    string SecretKey = AuthService.GenerateSecretKey();
                    string qrCodeUri = AuthService.GenerateQrCodeUri(NewAdmin.Email, SecretKey);
                    await AuthService.EnableTwoFactorAuthenticationAsync(NewAdmin.Email, SecretKey);
                    ModalParams.Add("Email", NewAdmin.Email);
                    ModalParams.Add("SecretKey", SecretKey);
                    ModalParams.Add("QrCodeImage", AuthService.GenerateQrCodeImage(qrCodeUri));
                    IModalReference emailInUseModal = Modal.Show<Setup2FaModal>("Twee-factor-authenticatie instellen", ModalParams, options);
                    ModalResult result = await emailInUseModal.Result;
                    if (result.Confirmed)
                    {
                        ToastService.ShowSuccess("Twee-factor-authenticatie klaarzetten gelukt");
                        navigationManager.NavigateTo("/", true);
                    }
                }
            }
        }

        public class AuthModel
        {
            [RequiredCustom("Email")]
            [RegularExpression(@"^[\w-\.]+@([\w-]+\.)+[\w-]{2,4}$", ErrorMessage = "Email is required")]
            public string Email { get; set; } = string.Empty;

            [RequiredCustom("Password")]
            [Password(ErrorMessage = "You need to have atleast 1 uppercase, 1 lowercase, 1 special character, 1 number")]
            [MinLength(15, ErrorMessage = "Password needs to be minimale 15 characters long")]
            public string Password { get; set; } = string.Empty;

            [RequiredCustom("Confirming Password")]
            [Compare("Password", ErrorMessage = "The password doesn't match, please try again.")]
            public string CPassword { get; set; } = string.Empty;
        }
    }
}
