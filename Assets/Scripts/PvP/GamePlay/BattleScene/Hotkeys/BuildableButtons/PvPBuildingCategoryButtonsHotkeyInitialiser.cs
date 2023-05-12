using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.UI.BattleScene.Buttons;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Utils;
using UnityEngine;
using UnityEngine.Assertions;

namespace BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Hotkeys.BuildableButtons
{
    public class PvPBuildingCategoryButtonsHotkeyInitialiser : MonoBehaviour
    {
        private PvPDummyBuildingCategoryButton _nullButton;

        // Keep references to avoid garbage collection
        private IPvPManagedDisposable _buildingCategoryHotkeyListener;

        public PvPBuildingCategoryButton factoriesButton, defensivesButton, offensivesButton, tacticalsButton, ultrasButton;

        void Awake()
        {
            // Locked buttons may be destroyed, so null check before they are destroyed
            PvPHelper.AssertIsNotNull(factoriesButton, defensivesButton, offensivesButton, tacticalsButton, ultrasButton);

            _nullButton = new PvPDummyBuildingCategoryButton();
        }

        public void Initialise(IPvPHotkeyDetector hotkeyDetector)
        {
            Assert.IsNotNull(hotkeyDetector);

            _buildingCategoryHotkeyListener
                = new PvPBuildingCategoryHotkeyListener(
                    hotkeyDetector,
                    factoriesButton,
                    defensivesButton,
                    offensivesButton,
                    UseNullButtonIfNeeded(tacticalsButton),
                    UseNullButtonIfNeeded(ultrasButton));
        }

        // Locked buttons may have been destroyed, so replace these with a dummy button
        private IPvPBuildingCategoryButton UseNullButtonIfNeeded(PvPBuildingCategoryButton realButton)
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