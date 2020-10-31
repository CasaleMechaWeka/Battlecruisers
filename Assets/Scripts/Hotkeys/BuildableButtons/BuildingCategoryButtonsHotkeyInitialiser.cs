using BattleCruisers.Hotkeys;
using BattleCruisers.Hotkeys.BuildableButtons;
using BattleCruisers.UI.BattleScene.Buttons;
using BattleCruisers.Utils;
using UnityEngine;
using UnityEngine.Assertions;

namespace Assets.Scripts.Hotkeys.BuildableButtons
{
    public class BuildingCategoryButtonsHotkeyInitialiser : MonoBehaviour
    {
        // Keep references to avoid garbage collection
        private IManagedDisposable _buildingCategoryHotkeyListener;

        public BuildingCategoryButton factoriesButton, defensivesButton, offensivesButton, tacticalsButton, ultrasButton;

        public void Initialise(IHotkeyDetector hotkeyDetector)
        {
            Helper.AssertIsNotNull(factoriesButton, defensivesButton, offensivesButton, tacticalsButton, ultrasButton);
            Assert.IsNotNull(hotkeyDetector);

            _buildingCategoryHotkeyListener = new BuildingCategoryHotkeyListener(hotkeyDetector, factoriesButton, defensivesButton, offensivesButton, tacticalsButton, ultrasButton);
        }
    }
}