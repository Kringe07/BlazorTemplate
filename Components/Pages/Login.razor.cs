﻿using Blazored.Modal;
using Blazored.Modal.Services;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;
using ProjectName.Authentication;
using ProjectName.Components.Modal;
using ProjectName.DataAccess.Entities;
using ProjectName.DataAccess.Repository;
using BC = BCrypt.Net.BCrypt;

namespace ProjectName.Components.Pages
{
    public partial class Login
    {
        [CascadingParameter] public IModalService Modal { get; set; } = default!;
        [Inject] private AuthenticationStateProvider StateProvider { get; set; } = default!;
        [Inject] private NavigationManager navigationManager { get; set; } = default!;
        [Inject] private UserRepository UserRepository { get; set; } = default!;

        public class Model
        {
            public string Email = string.Empty;

            public string Password = string.Empty;
        }

        private string error = string.Empty;

        private Model model = new Model();

        private void FailedAttempted()
        {
            LoginAttemptService.RecordFailedAttempt(model.Email);
            if (LoginAttemptService.IsUserLockedOut(model.Email))
            {
                error = "Je bent buitengesloten vanwege te veel mislukte inlogpogingen.";
            }
            else
            {
                error = "Foutieve Email, wachtwoord combinatie.";
            }
        }
        private async Task Authenticate()
        {

            if (LoginAttemptService.IsUserLockedOut(model.Email))
            {
                var timeRemaining = LoginAttemptService.GetLockoutTimeRemaining(model.Email);
                error = $"Je bent buitengesloten. probeer later opniew:{timeRemaining?.Minutes} minutes.";
                StateHasChanged();
                return;
            }
            User? UserAccount = await UserRepository.GetUserDataAsync(model.Email);
            CustomAuthentication customAuthentication = (CustomAuthentication)StateProvider;
            ////No check for customers
            //var customAuthentication = (CustomAuthentication)StateProvider;
            //if (UserAccount is Customer customer)
            //{
            //    if (customer.IsBlocked)
            //    {
            //        error = "Account is geblokkeerd";
            //        StateHasChanged();
            //        return;
            //    }

            //    await customAuthentication.UpdateAuthenticationState(new UserSession()
            //    {
            //        Email = UserAccount.Email,
            //        Role = UserAccount.Role.ToString(),
            //    });
            //    navigationManager.NavigateTo("/");
            //}

            if (UserAccount == null || !BC.EnhancedVerify(model.Password, UserAccount.Password))
            {
                FailedAttempted();
                return;
            }
            ModalParameters ModalParams = new();
            ModalParams.Add("Email", UserAccount.Email);
            IModalReference emailInUseModal = Modal.Show<Verify2FaModal>("Twee-factor-authenticatiecode valideren", ModalParams);
            ModalResult result = await emailInUseModal.Result;
            if (result.Confirmed)
            {
                ToastService.ShowSuccess("Twee-factor-authenticatie goedgekeurd");
                await customAuthentication.UpdateAuthenticationState(new UserSession()
                {
                    Email = UserAccount.Email,
                    Role = UserAccount.Role.ToString(),
                });

                navigationManager.NavigateTo("/");
            }
        }
    }
}
