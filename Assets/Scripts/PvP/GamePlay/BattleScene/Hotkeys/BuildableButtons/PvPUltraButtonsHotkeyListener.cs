using BattleCruisers.Hotkeys;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.UI.BattleScene.Buttons;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Utils;
using System;

namespace BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Hotkeys.BuildableButtons
{
    public class PvPUltraButtonsHotkeyListener : PvPBuildableButtonHotkeyListener, IPvPManagedDisposable
    {
        private readonly IPvPBuildableButton _deathstarButton, _nukeLauncherButton, _ultraliskButton, _kamikazeSignalButton, _broadsidesButton;

        public PvPUltraButtonsHotkeyListener(
            IHotkeyDetector hotkeyDetector,
            IPvPBuildableButton deathstarButton,
            IPvPBuildableButton nukeLauncherButton,
            IPvPBuildableButton ultraliskButton,
            IPvPBuildableButton kamikazeSignalButton,
            IPvPBuildableButton broadsidesButton)
            : base(hotkeyDetector)
        {
            PvPHelper.AssertIsNotNull(deathstarButton, nukeLauncherButton, ultraliskButton, kamikazeSignalButton, broadsidesButton);

            _deathstarButton = deathstarButton;
            _nukeLauncherButton = nukeLauncherButton;
            _ultraliskButton = ultraliskButton;
            _kamikazeSignalButton = kamikazeSignalButton;
            _broadsidesButton = broadsidesButton;

            _hotkeyDetector.UltraButton1 += _hotkeyDetector_Deathstar;
            _hotkeyDetector.UltraButton2 += _hotkeyDetector_NukeLauncher;
            _hotkeyDetector.UltraButton3 += _hotkeyDetector_Ultralisk;
            _hotkeyDetector.UltraButton4 += _hotkeyDetector_KamikazeSignal;
            _hotkeyDetector.UltraButton5 += _hotkeyDetector_Broadsides;
        }

        private void _hotkeyDetector_Deathstar(object sender, EventArgs e)
        {
            ClickIfPresented(_deathstarButton);
        }

        private void _hotkeyDetector_NukeLauncher(object sender, EventArgs e)
        {
            ClickIfPresented(_nukeLauncherButton);
        }

        private void _hotkeyDetector_Ultralisk(object sender, EventArgs e)
        {
            ClickIfPresented(_ultraliskButton);
        }

        private void _hotkeyDetector_KamikazeSignal(object sender, EventArgs e)
        {
            ClickIfPresented(_kamikazeSignalButton);
        }

        private void _hotkeyDetector_Broadsides(object sender, EventArgs e)
        {
            ClickIfPresented(_broadsidesButton);
        }

        public void DisposeManagedState()
        {
            _hotkeyDetector.UltraButton1 -= _hotkeyDetector_Deathstar;
            _hotkeyDetector.UltraButton2 -= _hotkeyDetector_NukeLauncher;
            _hotkeyDetector.UltraButton3 -= _hotkeyDetector_Ultralisk;
            _hotkeyDetector.UltraButton4 -= _hotkeyDetector_KamikazeSignal;
            _hotkeyDetector.UltraButton5 -= _hotkeyDetector_Broadsides;
        }
    }
}