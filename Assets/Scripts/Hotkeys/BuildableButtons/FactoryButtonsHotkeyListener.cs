using BattleCruisers.UI.BattleScene.Buttons;
using BattleCruisers.Utils;
using System;

namespace BattleCruisers.Hotkeys.BuildableButtons
{
    public class FactoryButtonsHotkeyListener : BuildableButtonHotkeyListener, IManagedDisposable
    {
        private IBuildableButton _droneStationButton, _airFactoryButton, _navalFactoryButtons, _droneStation4Button, _droneStation8Button;

        public FactoryButtonsHotkeyListener(
            IHotkeyDetector hotkeyDetector,
            IBuildableButton droneStationButton,
            IBuildableButton airFactoryButton,
            IBuildableButton navalFactoryButton,
            IBuildableButton droneStation4Button,
            IBuildableButton droneStation8Button)
            : base(hotkeyDetector)
        {
            Helper.AssertIsNotNull(droneStationButton, airFactoryButton, navalFactoryButton);

            _droneStationButton = droneStationButton;
            _airFactoryButton = airFactoryButton;
            _navalFactoryButtons = navalFactoryButton;
            _droneStation4Button = droneStation4Button;
            _droneStation8Button = droneStation8Button;

            _hotkeyDetector.FactoryButton1 += _hotkeyDetector_DroneStation;
            _hotkeyDetector.FactoryButton2 += _hotkeyDetector_AirFactory;
            _hotkeyDetector.FactoryButton3 += _hotkeyDetector_NavalFactory;
            _hotkeyDetector.FactoryButton4 += _hotkeyDetector_DroneStation4;
            _hotkeyDetector.FactoryButton5 += _hotkeyDetector_DroneStation8;
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

        private void _hotkeyDetector_DroneStation4(object sender, EventArgs e)
        {
            ClickIfPresented(_droneStation4Button);
        }

        private void _hotkeyDetector_DroneStation8(object sender, EventArgs e)
        {
            ClickIfPresented(_droneStation8Button);
        }

        public void DisposeManagedState()
        {
            _hotkeyDetector.FactoryButton1 -= _hotkeyDetector_DroneStation;
            _hotkeyDetector.FactoryButton2 -= _hotkeyDetector_AirFactory;
            _hotkeyDetector.FactoryButton3 -= _hotkeyDetector_NavalFactory;
            _hotkeyDetector.FactoryButton4 -= _hotkeyDetector_DroneStation4;
            _hotkeyDetector.FactoryButton5 -= _hotkeyDetector_DroneStation8;
        }
    }
}