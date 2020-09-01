using UnityEngine.Assertions;

namespace BattleCruisers.UI.BattleScene.GameSpeed
{
    // FELIX  Remove.  Unused :D
    /// <summary>
    /// Ensures only one speed button is selected at a time.  Simply
    /// deselects the previously selected speed button when a new button
    /// is selected.
    /// </summary>
    public class SpeedButtonManager : ISpeedButtonManager
    {
        private IGameSpeedButton _selectedButton;
        private IGameSpeedButton SelectedButton
        {
            get { return _selectedButton; }
            set
            {
                Assert.IsNotNull(value);

                if (_selectedButton != null)
                {
                    _selectedButton.IsSelected = false;
                }
                _selectedButton = value;
                _selectedButton.IsSelected = true;
            }
        }

        public void SelectButton(IGameSpeedButton speedButton)
        {
            SelectedButton = speedButton;
        }
    }
}
