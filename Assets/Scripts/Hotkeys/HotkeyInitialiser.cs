using BattleCruisers.UI.BattleScene.Navigation;
using BattleCruisers.Utils;
using BattleCruisers.Utils.BattleScene.Update;
using BattleCruisers.Utils.PlatformAbstractions;

namespace BattleCruisers.Hotkeys
{
    // FELIX  Use, test
    public class HotkeyInitialiser
    {
        // Keep references to avoid garbage collection
        private readonly NavigationHotkeyListener _navigationHotkeyListener;

        public HotkeyInitialiser(
            IHotkeyList hotkeyList,
            IInput input,
            IUpdater updater,
            ICameraFocuser cameraFocuser)
        {
            Helper.AssertIsNotNull(hotkeyList, input, updater, cameraFocuser);

            IHotkeyDetector hotkeyDetector = new HotkeyDetector(hotkeyList, input, updater);
            _navigationHotkeyListener = new NavigationHotkeyListener(hotkeyDetector, cameraFocuser);
        }
    }
}