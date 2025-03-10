using BattleCruisers.Hotkeys;
using BattleCruisers.Hotkeys.Escape;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Hotkeys.BuildableButtons;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Utils;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Utils.PlatformAbstractions;
using BattleCruisers.UI.BattleScene.MainMenu;
using BattleCruisers.UI.BattleScene.Navigation;
using BattleCruisers.UI.Filters;
using BattleCruisers.Utils.BattleScene.Update;
using BattleCruisers.Utils.PlatformAbstractions;
using UnityEngine;

namespace BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Hotkeys
{
    public class PvPHotkeyInitialiser : MonoBehaviour
    {
        // Keep references to avoid garbage collection
        private PvPNavigationHotkeyListener _navigationHotkeyListener;
        //   private PvPGameSpeedHotkeyListener _gameSpeedHotkeyListener;
        private EscapeHandler _escapeHandler;

        public PvPBuildableButtonsHotkeyInitialiser buildableButtonsHotkeyInitialiser;
        public PvPBuildingCategoryButtonsHotkeyInitialiser buildingCategoryButtonsHotkeyInitialiser;

        public void Initialise(
            IHotkeyList hotkeyList,
            IInput input,
            IUpdater updater,
            IBroadcastingFilter hotkeyFilter,
            ICameraFocuser cameraFocuser,
            //            IPvPSpeedComponents speedComponents,
            IMainMenuManager mainMenuManager)
        {
            PvPHelper.AssertIsNotNull(buildableButtonsHotkeyInitialiser, buildingCategoryButtonsHotkeyInitialiser);
            PvPHelper.AssertIsNotNull(hotkeyList, input, updater, hotkeyFilter, cameraFocuser, /*speedComponents,*/ mainMenuManager);

            // Hotkeys (only for PC)
            IHotkeyDetector hotkeyDetector = CreateHotkeyDetector(hotkeyList, input, updater, hotkeyFilter);

            _navigationHotkeyListener = new PvPNavigationHotkeyListener(hotkeyDetector, cameraFocuser);
            //    _gameSpeedHotkeyListener = new PvPGameSpeedHotkeyListener(hotkeyDetector/*, speedComponents*/);
            buildableButtonsHotkeyInitialiser.Initialise(hotkeyDetector);
            buildingCategoryButtonsHotkeyInitialiser.Initialise(hotkeyDetector);

            // Escape (all platforms)
            IEscapeDetector escapeDetector = new EscapeDetector(input, updater);
            _escapeHandler = new EscapeHandler(escapeDetector, mainMenuManager);
        }

        private IHotkeyDetector CreateHotkeyDetector(IHotkeyList hotkeyList, IInput input, IUpdater updater, IBroadcastingFilter hotkeyFilter)
        {
            if (PvPSystemInfoBC.Instance.IsHandheld)
                // Handheld devices have no hotkeys :)
                return new NullHotkeyDetector();
            else
                return new HotkeyDetector(hotkeyList, input, updater, hotkeyFilter);
        }
    }
}