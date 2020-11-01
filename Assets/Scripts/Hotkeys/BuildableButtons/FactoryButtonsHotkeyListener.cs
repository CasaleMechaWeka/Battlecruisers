using BattleCruisers.UI.BattleScene.Buttons;
using BattleCruisers.Utils;
using System;

namespace BattleCruisers.Hotkeys.BuildableButtons
{
    public class FactoryButtonsHotkeyListener : BuildableButtonHotkeyListener, IManagedDisposable
    {
        private readonly IBuildableButton _droneStationButton, _airFactoryButton, _navalFactoryButtons;

        public FactoryButtonsHotkeyListener(
            IHotkeyDetector hotkeyDetector,
            IBuildableButton droneStationButton,
            IBuildableButton airFactoryButton,
            IBuildableButton navalFactoryButton)
            : base(hotkeyDetector)
        {
            Helper.AssertIsNotNull(droneStationButton, airFactoryButton, navalFactoryButton);

            _droneStationButton = droneStationButton;
            _airFactoryButton = airFactoryButton;
            _navalFactoryButtons = navalFactoryButton;

            _hotkeyDetector.DroneStation += _hotkeyDetector_DroneStation;
            _hotkeyDetector.AirFactory += _hotkeyDetector_AirFactory;
            _hotkeyDetector.NavalFactory += _hotkeyDetector_NavalFactory;
        }

        private void _hotkeyDetector_DroneStation(object sender, EventArgs e)
        {
            ClickIfPresented(_droneStationButton);
        }

        private void _hotkeyDetector_AirFactory(object sender, EventArgs e)
        {
            ClickIfPresented(_airFactoryButton);
        }

        private void _hotkeyDetector_NavalFactory(object sender, EventArgs e)
        {
            ClickIfPresented(_navalFactoryButtons);
        }

        public void DisposeManagedState()
        {
            _hotkeyDetector.DroneStation -= _hotkeyDetector_DroneStation;
            _hotkeyDetector.AirFactory -= _hotkeyDetector_AirFactory;
            _hotkeyDetector.NavalFactory -= _hotkeyDetector_NavalFactory;
        }
    }
}