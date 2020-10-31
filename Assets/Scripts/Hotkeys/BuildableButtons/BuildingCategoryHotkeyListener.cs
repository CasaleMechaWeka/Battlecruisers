using BattleCruisers.UI.BattleScene.Buttons;
using BattleCruisers.Utils;
using System;

// FELIX  Use, test
namespace BattleCruisers.Hotkeys.BuildableButtons
{
    public class BuildingCategoryHotkeyListener : IManagedDisposable
    {
        private readonly IHotkeyDetector _hotkeyDetector;
        private readonly IBuildingCategoryButton _factoriesButton, _defensivesButton, _offensivesButton, _tacticalsButton, _ultrasButton;

        public BuildingCategoryHotkeyListener(
            IHotkeyDetector hotkeyDetector,
            IBuildingCategoryButton factoriesButton,
            IBuildingCategoryButton defensivesButton,
            IBuildingCategoryButton offensivesButton,
            IBuildingCategoryButton tacticalsButton,
            IBuildingCategoryButton ultrasButton)
        {
            Helper.AssertIsNotNull(hotkeyDetector, factoriesButton, defensivesButton, offensivesButton, tacticalsButton, ultrasButton);

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