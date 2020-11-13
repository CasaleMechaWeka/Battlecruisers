using Assets.Scripts.Hotkeys.BuildableButtons;
using BattleCruisers.UI.BattleScene.GameSpeed;
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

        public BuildableButtonsHotkeyInitialiser buildableButtonsHotkeyInitialiser;
        public BuildingCategoryButtonsHotkeyInitialiser buildingCategoryButtonsHotkeyInitialiser;

        public void Initialise(
            IHotkeyList hotkeyList,
            IInput input,
            IUpdater updater,
            IBroadcastingFilter hotkeyFilter,
            ICameraFocuser cameraFocuser,
            ISpeedComponents speedComponents)
        {
            Helper.AssertIsNotNull(buildableButtonsHotkeyInitialiser, buildingCategoryButtonsHotkeyInitialiser);
            Helper.AssertIsNotNull(hotkeyList, input, updater, hotkeyFilter, cameraFocuser, speedComponents);
            
            IHotkeyDetector hotkeyDetector = CreateHotkeyDetector(hotkeyList, input, updater, hotkeyFilter);

            _navigationHotkeyListener = new NavigationHotkeyListener(hotkeyDetector, cameraFocuser);
            _gameSpeedHotkeyListener = new GameSpeedHotkeyListener(hotkeyDetector, speedComponents);
            buildableButtonsHotkeyInitialiser.Initialise(hotkeyDetector);
            buildingCategoryButtonsHotkeyInitialiser.Initialise(hotkeyDetector);
        }

        private IHotkeyDetector CreateHotkeyDetector(IHotkeyList hotkeyList, IInput input, IUpdater updater, IBroadcastingFilter hotkeyFilter)
        {
            if (SystemInfoBC.Instance.IsHandheld)
            {
                // Handheld devices have no hotkeys :)
                return new NullHotkeyDetector();
            }
            else
            {
                return new HotkeyDetector(hotkeyList, input, updater, hotkeyFilter);
            }
        }
    }
}