using BattleCruisers.Hotkeys;
using BattleCruisers.Hotkeys.BuildableButtons;
using BattleCruisers.UI.BattleScene.Buttons;
using BattleCruisers.Utils;
using UnityEngine;
using UnityEngine.Assertions;

// FELIX  fix :D
namespace Assets.Scripts.Hotkeys.BuildableButtons
{
    public class BuildingCategoryButtonsHotkeyInitialiser : MonoBehaviour
    {
        private DummyBuildingCategoryButton _nullButton;

        // Keep references to avoid garbage collection
        private IManagedDisposable _buildingCategoryHotkeyListener;

        public BuildingCategoryButton factoriesButton, defensivesButton, offensivesButton, tacticalsButton, ultrasButton;

        void Awake()
        {
            // Locked buttons may be destroyed, so null check before they are destroyed
            Helper.AssertIsNotNull(factoriesButton, defensivesButton, offensivesButton, tacticalsButton, ultrasButton);

            _nullButton = new DummyBuildingCategoryButton();
        }

        public void Initialise(IHotkeyDetector hotkeyDetector)
        {
            Assert.IsNotNull(hotkeyDetector);

            _buildingCategoryHotkeyListener 
                = new BuildingCategoryHotkeyListener(
                    hotkeyDetector, 
                    factoriesButton, 
                    defensivesButton, 
                    offensivesButton, 
                    UseNullButtonIfNeeded(tacticalsButton), 
                    UseNullButtonIfNeeded(ultrasButton));
        }

        // Locked buttons may have been destroyed, so replace these with a dummy button
        private IBuildingCategoryButton UseNullButtonIfNeeded(BuildingCategoryButton realButton)
        {
            // Destroyed Monobehaviour == null. Destroyed interface (eg: ), != null. => Need Monobehaviour (BuildableButtonController)
            if (realButton != null)
            {
                return realButton;
            }
            else
            {
                return _nullButton;
            }
        }
    }
}