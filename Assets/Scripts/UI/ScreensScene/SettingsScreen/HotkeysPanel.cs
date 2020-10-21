using BattleCruisers.UI.Panels;
using BattleCruisers.Utils.PlatformAbstractions;
using UnityEngine;
using UnityEngine.Assertions;

namespace BattleCruisers.UI.ScreensScene.SettingsScreen
{
    public class HotkeysPanel : Panel
    {
        public HotkeyRow playerCruiserRow;

        public void Initialise()
        {
            Assert.IsNotNull(playerCruiserRow);
            playerCruiserRow.Initialise(InputBC.Instance, KeyCode.LeftArrow);
        }
    }
}