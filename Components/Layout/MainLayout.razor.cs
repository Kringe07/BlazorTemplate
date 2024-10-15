namespace ProjectName.Components.Layout
{
    public partial class MainLayout
    {
        private bool WantTopbar { get; set; } = false;
        private bool _iconMenuActive { get; set; }
        private string? IconMenuCssClass => _iconMenuActive ? "width: 80px;" : null;

        protected override void OnInitialized()
        {
            WantTopbar = Configuration.GetValue<bool>("Topbar");
        }

        protected void ToggleIconMenu(bool iconMenuActive)
        {
            _iconMenuActive = iconMenuActive;
        }
    }
}
