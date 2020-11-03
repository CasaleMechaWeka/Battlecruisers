using Assets.Scripts.Hotkeys.BuildableButtons;
using BattleCruisers.UI.BattleScene.Navigation;
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

        public BuildableButtonsHotkeyInitialiser buildableButtonsHotkeyInitialiser;
        public BuildingCategoryButtonsHotkeyInitialiser buildingCategoryButtonsHotkeyInitialiser;

        public void Initialise(
            IHotkeyList hotkeyList,
            IInput input,
            IUpdater updater,
            ICameraFocuser cameraFocuser)
        {
            Helper.AssertIsNotNull(buildableButtonsHotkeyInitialiser, buildingCategoryButtonsHotkeyInitialiser);
            Helper.AssertIsNotNull(hotkeyList, input, updater, cameraFocuser);
            
            IHotkeyDetector hotkeyDetector = CreateHotkeyDetector(hotkeyList, input, updater);

            _navigationHotkeyListener = new NavigationHotkeyListener(hotkeyDetector, cameraFocuser);
            buildableButtonsHotkeyInitialiser.Initialise(hotkeyDetector);
            buildingCategoryButtonsHotkeyInitialiser.Initialise(hotkeyDetector);
        }

        private IHotkeyDetector CreateHotkeyDetector(IHotkeyList hotkeyList, IInput input, IUpdater updater)
        {
            if (SystemInfoBC.Instance.IsHandheld)
            {
                // Handheld devices have no hotkeys :)
                return new NullHotkeyDetector();
            }
            else
            {
                return new HotkeyDetector(hotkeyList, input, updater);
            }
        }
    }
}