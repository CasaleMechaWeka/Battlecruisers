using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.UI.BattleScene.Buttons;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Utils;
using System;

namespace BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Hotkeys.BuildableButtons
{
    public class PvPBuildingCategoryHotkeyListener : IPvPManagedDisposable
    {
        private readonly IPvPHotkeyDetector _hotkeyDetector;
        private readonly IPvPBuildingCategoryButton _factoriesButton, _defensivesButton, _offensivesButton, _tacticalsButton, _ultrasButton;

        public PvPBuildingCategoryHotkeyListener(
            IPvPHotkeyDetector hotkeyDetector,
            IPvPBuildingCategoryButton factoriesButton,
            IPvPBuildingCategoryButton defensivesButton,
            IPvPBuildingCategoryButton offensivesButton,
            IPvPBuildingCategoryButton tacticalsButton,
            IPvPBuildingCategoryButton ultrasButton)
        {
            PvPHelper.AssertIsNotNull(hotkeyDetector, factoriesButton, defensivesButton, offensivesButton, tacticalsButton, ultrasButton);

            _hotkeyDetector = hotkeyDetector;
            _factoriesButton = factoriesButton;
            _defensivesButton = defensivesButton;
            _offensivesButton = offensivesButton;
            _tacticalsButton = tacticalsButton;
            _ultrasButton = ultrasButton;

            _hotkeyDetector.Factories += _hotkeyDetector_Factories;
            _hotkeyDetector.Defensives += _hotkeyDetector_Defensives;
            _hotkeyDetector.Offensives += _hotkeyDetector_Offensives;
            _hotkeyDetector.Tacticals += _hotkeyDetector_Tacticals;
            _hotkeyDetector.Ultras += _hotkeyDetector_Ultras;
        }

        private void _hotkeyDetector_Factories(object sender, EventArgs e)
        {
            _factoriesButton.TriggerClick();
        }

        private void _hotkeyDetector_Defensives(object sender, EventArgs e)
        {
            _defensivesButton.TriggerClick();
        }

        private void _hotkeyDetector_Offensives(object sender, EventArgs e)
        {
            _offensivesButton.TriggerClick();
        }

        private void _hotkeyDetector_Tacticals(object sender, EventArgs e)
        {
            _tacticalsButton.TriggerClick();
        }

        private void _hotkeyDetector_Ultras(object sender, EventArgs e)
        {
            _ultrasButton.TriggerClick();
        }

        public void DisposeManagedState()
        {
            _hotkeyDetector.Factories -= _hotkeyDetector_Factories;
            _hotkeyDetector.Defensives -= _hotkeyDetector_Defensives;
            _hotkeyDetector.Offensives -= _hotkeyDetector_Offensives;
            _hotkeyDetector.Tacticals -= _hotkeyDetector_Tacticals;
            _hotkeyDetector.Ultras -= _hotkeyDetector_Ultras;
        }
    }
}