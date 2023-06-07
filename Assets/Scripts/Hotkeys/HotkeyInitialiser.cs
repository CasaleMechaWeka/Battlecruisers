using BattleCruisers.Hotkeys.BuildableButtons;
using BattleCruisers.Hotkeys.Escape;
using BattleCruisers.UI.BattleScene.GameSpeed;
using BattleCruisers.UI.BattleScene.HelpLabels;
using BattleCruisers.UI.BattleScene.MainMenu;
using BattleCruisers.UI.BattleScene.Manager;
using BattleCruisers.UI.BattleScene.Navigation;
using BattleCruisers.UI.Filters;
using BattleCruisers.Utils;
using BattleCruisers.Utils.BattleScene.Update;
using BattleCruisers.Utils.PlatformAbstractions;
using UnityEngine;

namespace BattleCruisers.Hotkeys
{
    public class HotkeyInitialiser : MonoBehaviour
    {
        // Keep references to avoid garbage collection
        private NavigationHotkeyListener _navigationHotkeyListener;
        private GameSpeedHotkeyListener _gameSpeedHotkeyListener;
        private EscapeHandler _escapeHandler;

        public BuildableButtonsHotkeyInitialiser buildableButtonsHotkeyInitialiser;
        public BuildingCategoryButtonsHotkeyInitialiser buildingCategoryButtonsHotkeyInitialiser;

        public void Initialise(
            IHotkeyList hotkeyList,
            IInput input,
            IUpdater updater,
            IBroadcastingFilter hotkeyFilter,
            ICameraFocuser cameraFocuser,
            ISpeedComponents speedComponents,
            IMainMenuManager mainMenuManager,
            IUIManager uIManager)
        {
            Helper.AssertIsNotNull(buildableButtonsHotkeyInitialiser, buildingCategoryButtonsHotkeyInitialiser);
            Helper.AssertIsNotNull(hotkeyList, input, updater, hotkeyFilter, cameraFocuser, speedComponents, mainMenuManager);
            
            // Hotkeys (only for PC)
            IHotkeyDetector hotkeyDetector = CreateHotkeyDetector(hotkeyList, input, updater, hotkeyFilter, uIManager);

            _navigationHotkeyListener = new NavigationHotkeyListener(hotkeyDetector, cameraFocuser);
            _gameSpeedHotkeyListener = new GameSpeedHotkeyListener(hotkeyDetector, speedComponents);
            buildableButtonsHotkeyInitialiser.Initialise(hotkeyDetector);
            buildingCategoryButtonsHotkeyInitialiser.Initialise(hotkeyDetector);

            // Escape (all platforms)
            IEscapeDetector escapeDetector = new EscapeDetector(input, updater);
            _escapeHandler = new EscapeHandler(escapeDetector, mainMenuManager);
        }

        private IHotkeyDetector CreateHotkeyDetector(IHotkeyList hotkeyList, IInput input, IUpdater updater, IBroadcastingFilter hotkeyFilter, IUIManager uIManager)
        {
            if (SystemInfoBC.Instance.IsHandheld)
            {
                // Handheld devices have no hotkeys :)
                return new NullHotkeyDetector();
            }
            else
            {
                return new HotkeyDetector(hotkeyList, input, updater, hotkeyFilter, uIManager);
            }
        }
    }
}