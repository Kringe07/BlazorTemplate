using Blazored.Modal;
using Blazored.Toast;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.EntityFrameworkCore;
using ProjectName.Authentication;
using ProjectName.Components;
using ProjectName.DataAccess.DatabaseContext;
using ProjectName.DataAccess.Repository;
using ProjectName.Seeder;
using ProjectName.Service;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddRazorComponents().AddInteractiveServerComponents();
builder.Services.AddCascadingAuthenticationState();
builder.Services.AddBlazoredModal();
builder.Services.AddBlazoredToast();
builder.Services.AddAuthenticationCore();
builder.Services.AddSingleton<IAuthorizationMiddlewareResultHandler, AuthorizationMiddlewareResultHandler>();
builder.Services.AddScoped<AuthenticationStateProvider, CustomAuthentication>();
builder.Services.AddDbContextFactory<ProjectNameContext>(options =>
{
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"));
});
builder.Services.AddScoped<UserRepository>();
builder.Services.AddScoped<LoginAttemptService>();
builder.Services.AddScoped<AuthenticationService>();
var app = builder.Build();

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error", createScopeForErrors: true);
    app.UseHsts();
}

using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;

    DumSeederDBInitializer.Seed(services);
}
app.UseStatusCodePagesWithRedirects("/404");
app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseAntiforgery();
app.MapRazorComponents<App>()
    .AddInteractiveServerRenderMode();

app.Run();