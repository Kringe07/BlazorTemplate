namespace ProjectName.Components.Layout;

public partial class MainLayout
{
    private bool IconMenuActive { get; set; }
    private string? IconMenuCssClass => IconMenuActive ? "width: 80px;" : null;

    protected override void OnInitialized()
    {
        Configuration.GetValue<bool>("Topbar");
    }

    protected void ToggleIconMenu(bool iconMenuActive)
    {
        IconMenuActive = iconMenuActive;
    }
}