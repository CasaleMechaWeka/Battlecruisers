using Assets.Scripts.Hotkeys.BuildableButtons;
using BattleCruisers.UI.BattleScene.Navigation;
using BattleCruisers.Utils;
using BattleCruisers.Utils.BattleScene.Update;
using BattleCruisers.Utils.PlatformAbstractions;
using UnityEngine;
using UnityEngine.Assertions;

namespace BattleCruisers.Hotkeys
{
    public class HotkeyInitialiser : MonoBehaviour
    {
        // Keep references to avoid garbage collection
        private NavigationHotkeyListener _navigationHotkeyListener;

        public BuildableButtonsHotkeyInitialiser buildableButtonsHotkeyInitialiser;

        public void Initialise(
            IHotkeyList hotkeyList,
            IInput input,
            IUpdater updater,
            ICameraFocuser cameraFocuser)
        {
            Assert.IsNotNull(buildableButtonsHotkeyInitialiser);
            Helper.AssertIsNotNull(hotkeyList, input, updater, cameraFocuser);

            IHotkeyDetector hotkeyDetector = new HotkeyDetector(hotkeyList, input, updater);
            _navigationHotkeyListener = new NavigationHotkeyListener(hotkeyDetector, cameraFocuser);
            buildableButtonsHotkeyInitialiser.Initialise(hotkeyDetector);
        }
    }
}