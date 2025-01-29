using Blazored.Modal;
using Blazored.Modal.Services;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;
using ProjectName.Authentication;
using ProjectName.Components.Modal;
using ProjectName.DataAccess.Entities;
using ProjectName.DataAccess.Repository;
using BC = BCrypt.Net.BCrypt;

namespace ProjectName.Components.Pages;

public partial class Login
{
    [CascadingParameter] public IModalService Modal { get; set; } = null!;
    [Inject] private AuthenticationStateProvider StateProvider { get; set; } = null!;
    [Inject] private NavigationManager NavigationManager { get; set; } = null!;
    [Inject] private UserRepository UserRepository { get; set; } = null!;
    private bool Enable2Fa { get; set; } = true;
    private readonly Model _model = new();
    private string _error = string.Empty;
        
    //Check Failed Attempts if you have tried 3 times u are locked out for 15min
    private void FailedAttempted()
    {
        LoginAttemptService.RecordFailedAttempt(_model.Email);
        _error = LoginAttemptService.IsUserLockedOut(_model.Email) ? "Je bent buitengesloten vanwege te veel mislukte inlogpogingen." 
            : "Foutieve Email, wachtwoord combinatie.";
    }

    //This is the Authentication system kinda
    // 1. check is user is not locked out else try later again
    // 2. check if user exists in the database and if the password is correct
    // 3. check if 2 FA is enabled [Yes] : Show 2 Fa verifaction modal [NO] : Skip the 2 Fa 
    // 4. Update Authentication and add set it in the session storage
    // 5. Reload Page So you can see what you can do on the website with your Roll
    private async Task Authenticate()
    {
        if (LoginAttemptService.IsUserLockedOut(_model.Email))
        {
            var timeRemaining = LoginAttemptService.GetLockoutTimeRemaining(_model.Email);
            _error = $"Je bent buitengesloten. probeer later opniew:{timeRemaining?.Minutes} minutes.";
            StateHasChanged();
            return;
        }
        var userAccount = await UserRepository.GetUserDataAsync(_model.Email);
        var customAuthentication = (CustomAuthentication)StateProvider;
        if (userAccount == null || !BC.EnhancedVerify(_model.Password, userAccount.Password))
        {
            FailedAttempted();
            return;
        }

        if (Enable2Fa)
        {
            var options = new ModalOptions
            {
                HideCloseButton = true,
                DisableBackgroundCancel = true
            };
            ModalParameters modalParams = new() { { "Email", userAccount.Email } };
            var emailInUseModal = Modal.Show<Verify2FaModal>("Twee-factor-authenticatiecode valideren", modalParams, options);
            var result = await emailInUseModal.Result;
            if (result.Confirmed)
            {
                ToastService.ShowSuccess("Twee-factor-authenticatie goedgekeurd");
                await customAuthentication.UpdateAuthenticationState(new UserSession
                {
                    Email = userAccount.Email,
                    Role = userAccount.RoleType.ToString()
                });

            }
        }
        else
        {
            await customAuthentication.UpdateAuthenticationState(new UserSession
            {
                Email = userAccount.Email,
                Role = userAccount.RoleType.ToString()
            });
        }
        NavigationManager.NavigateTo("/", true);
    }
}
    
public class Model
{
    public string Email = string.Empty;
    public string Password = string.Empty;
}