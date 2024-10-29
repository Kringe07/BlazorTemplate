using Blazored.Modal;
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

        private bool Enable2fa { get; set; } = true;

        private Model model = new Model();

        //Check Failed Attempts if you have tried 3 times u are locked out for 15min
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

        //This is the Authentication system kinda
        // 1. check is user is not locked out else try later again
        // 2. check if user exists in the database and if the password is correct
        // 3. check if 2 FA is enabled [Yes] : Show 2 Fa verifaction modal [NO] : Skip the 2 Fa 
        // 4. Update Authentication and add set it in the session storage
        // 5. Reload Page So you can see what you can do on the website with your Roll
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
            if (UserAccount == null || !BC.EnhancedVerify(model.Password, UserAccount.Password))
            {
                FailedAttempted();
                return;
            }

            if (Enable2fa)
            {
                var options = new ModalOptions()
                {
                    HideCloseButton = true,
                    DisableBackgroundCancel = true
                };
                ModalParameters ModalParams = new();
                ModalParams.Add("Email", UserAccount.Email);
                IModalReference emailInUseModal = Modal.Show<Verify2FaModal>("Twee-factor-authenticatiecode valideren", ModalParams, options);
                ModalResult result = await emailInUseModal.Result;
                if (result.Confirmed)
                {
                    ToastService.ShowSuccess("Twee-factor-authenticatie goedgekeurd");
                    await customAuthentication.UpdateAuthenticationState(new UserSession()
                    {
                        Email = UserAccount.Email,
                        Role = UserAccount.Role.ToString(),
                    });

                }
            }
            else
            {
                await customAuthentication.UpdateAuthenticationState(new UserSession()
                {
                    Email = UserAccount.Email,
                    Role = UserAccount.Role.ToString(),
                });
            }
            navigationManager.NavigateTo("/", true);
        }
    }
}
