using Blazored.Modal;
using Blazored.Modal.Services;
using Microsoft.AspNetCore.Components;
using ProjectName.Components.Modal;
using ProjectName.CustomAttribute;
using ProjectName.DataAccess.Entities;
using ProjectName.DataAccess.Repository;
using System.ComponentModel.DataAnnotations;
using BC = BCrypt.Net.BCrypt;

namespace ProjectName.Components.Pages;

public partial class Registration
{
    [CascadingParameter] public IModalService Modal { get; set; } = null!;
    [Inject] private NavigationManager NavigationManager { get; set; } = null!;
    [Inject] private AuthenticationService AuthService { get; set; } = null!;
    [Inject] private UserRepository UserRepository { get; set; } = null!;
    private Admin NewAdmin { get; } = new();
    public readonly AuthModel Auth = new();

    // This is kinda the registration system
    // 1. check if user email not already exist in the database
    // 2. set every thing in the new object class and save the user in the database
    // 3. show the 2 Fa and set it on
    // 4. show Toast when done and succeeded Return to main page
    private async Task Register()
    {
        var userAccount = await UserRepository.GetUserDataAsync(Auth.Email);
        if (userAccount == null)
        {
            NewAdmin.Password = BC.EnhancedHashPassword(Auth.Password);
            NewAdmin.Email = Auth.Email;
            NewAdmin.SecretKey = "";
            await UserRepository.AddUser(NewAdmin);
            if (await UserRepository.GetUserDataAsync(NewAdmin.Email) is not null)
            {
                var options = new ModalOptions
                {
                    HideCloseButton = true,
                    DisableBackgroundCancel = true
                };
                ModalParameters modalParams = new();
                var secretKey = AuthService.GenerateSecretKey();
                var qrCodeUri = AuthService.GenerateQrCodeUri(NewAdmin.Email, secretKey);
                await AuthService.EnableTwoFactorAuthenticationAsync(NewAdmin.Email, secretKey);
                modalParams.Add("Email", NewAdmin.Email);
                modalParams.Add("SecretKey", secretKey);
                modalParams.Add("QrCodeImage", AuthService.GenerateQrCodeImage(qrCodeUri));
                var emailInUseModal = Modal.Show<Setup2FaModal>("Twee-factor-authenticatie instellen", modalParams, options);
                var result = await emailInUseModal.Result;
                if (result.Confirmed)
                {
                    ToastService.ShowSuccess("Twee-factor-authenticatie klaarzetten gelukt");
                    NavigationManager.NavigateTo("/", true);
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