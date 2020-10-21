using BattleCruisers.UI.Panels;

namespace BattleCruisers.UI.ScreensScene.SettingsScreen
{
    public class HotkeysPanel : Panel
    {
        public void Initialise()
        {
            HotkeyRow[] rows = GetComponentsInChildren<HotkeyRow>();

            foreach (HotkeyRow row in rows)
            {
                row.Initialise();
            }
        }
    }
}