using BattleCruisers.Hotkeys;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Hotkeys.BuildableButtons;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Hotkeys.Escape;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.UI.BattleScene.GameSpeed;
// using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.UI.BattleScene.HelpLabels;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.UI.BattleScene.MainMenu;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.UI.BattleScene.Manager;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.UI.BattleScene.Navigation;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.UI.Filters;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Utils;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Utils.BattleScene.Update;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Utils.PlatformAbstractions;
using UnityEngine;

namespace BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Hotkeys
{
    public class PvPHotkeyInitialiser : MonoBehaviour
    {
        // Keep references to avoid garbage collection
        private PvPNavigationHotkeyListener _navigationHotkeyListener;
        private PvPGameSpeedHotkeyListener _gameSpeedHotkeyListener;
        private PvPEscapeHandler _escapeHandler;

        public PvPBuildableButtonsHotkeyInitialiser buildableButtonsHotkeyInitialiser;
        public PvPBuildingCategoryButtonsHotkeyInitialiser buildingCategoryButtonsHotkeyInitialiser;

        public void Initialise(
            IHotkeyList hotkeyList,
            IPvPInput input,
            IPvPUpdater updater,
            IPvPBroadcastingFilter hotkeyFilter,
            IPvPCameraFocuser cameraFocuser,
            IPvPSpeedComponents speedComponents,
            IPvPMainMenuManager mainMenuManager,
            IPvPUIManager uiManager)
        {
            PvPHelper.AssertIsNotNull(buildableButtonsHotkeyInitialiser, buildingCategoryButtonsHotkeyInitialiser);
            PvPHelper.AssertIsNotNull(hotkeyList, input, updater, hotkeyFilter, cameraFocuser, speedComponents, mainMenuManager);

            // Hotkeys (only for PC)
            IHotkeyDetector hotkeyDetector = CreateHotkeyDetector(hotkeyList, input, updater, hotkeyFilter, uiManager);

            _navigationHotkeyListener = new PvPNavigationHotkeyListener(hotkeyDetector, cameraFocuser);
            _gameSpeedHotkeyListener = new PvPGameSpeedHotkeyListener(hotkeyDetector, speedComponents);
            buildableButtonsHotkeyInitialiser.Initialise(hotkeyDetector);
            buildingCategoryButtonsHotkeyInitialiser.Initialise(hotkeyDetector);

            // Escape (all platforms)
            IPvPEscapeDetector escapeDetector = new PvPEscapeDetector(input, updater);
            _escapeHandler = new PvPEscapeHandler(escapeDetector, mainMenuManager);
        }

        private IHotkeyDetector CreateHotkeyDetector(IHotkeyList hotkeyList, IPvPInput input, IPvPUpdater updater, IPvPBroadcastingFilter hotkeyFilter, IPvPUIManager uiManager)
        {
            if (PvPSystemInfoBC.Instance.IsHandheld)
            {
                // Handheld devices have no hotkeys :)
                return new PvPNullHotkeyDetector();
            }
            else
            {
                return new PvPHotkeyDetector(hotkeyList, input, updater, hotkeyFilter, uiManager);
            }
        }
    }
}