using BattleCruisers.Hotkeys;
using BattleCruisers.Hotkeys.BuildableButtons;
using BattleCruisers.UI.BattleScene.Buttons;
using BattleCruisers.Utils;
using UnityEngine;
using UnityEngine.Assertions;

namespace Assets.Scripts.Hotkeys.BuildableButtons
{
    public class BuildableButtonsHotkeyInitialiser : MonoBehaviour
    {
        // Keep references to avoid garbage collection
        private IManagedDisposable _shipButtonsHotkeyListener;

        // FELIX
        //public BuildingButtonController
        public UnitButtonController attackBoatButton, frigateButton, destroyerButton, archonButton;

        public void Initialise(IHotkeyDetector hotkeyDetector)
        {
            Helper.AssertIsNotNull(attackBoatButton, frigateButton, destroyerButton, archonButton);
            Assert.IsNotNull(hotkeyDetector);

            _shipButtonsHotkeyListener = new ShipButtonsHotkeyListener(hotkeyDetector, attackBoatButton, frigateButton, destroyerButton, archonButton);
        }
    }
}