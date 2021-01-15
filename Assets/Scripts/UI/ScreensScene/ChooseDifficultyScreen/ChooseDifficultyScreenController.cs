using BattleCruisers.Data.Settings;
using BattleCruisers.Scenes;
using BattleCruisers.UI.Commands;
using BattleCruisers.UI.Common;
using BattleCruisers.UI.Sound;
using BattleCruisers.Utils;
using UnityEngine.Assertions;

namespace BattleCruisers.UI.ScreensScene.ChooseDifficultyScreen
{
    public class ChooseDifficultyScreenController : ScreenController, IChooseDifficultyScreen
    {
        private ISettingsManager _settingsManager;
        private Difficulty? _selectedDifficulty;
        private ICommand _startLevel1Command;

        public DifficultyButtonController harderButton, hardButton, normalButton, easyButton;
        public ButtonController startLevel1Button;

        public void Initialise(
            IScreensSceneGod screensSceneGod,
            ISingleSoundPlayer soundPlayer,
            ISettingsManager settingsManager)
        {
            base.Initialise(screensSceneGod);

            Helper.AssertIsNotNull(harderButton, hardButton, normalButton, easyButton, startLevel1Button);
            Helper.AssertIsNotNull(soundPlayer, settingsManager);

            _settingsManager = settingsManager;
            _selectedDifficulty = null;

            _startLevel1Command = new Command(StartLevel1, CanStartLevel1);
            startLevel1Button.Initialise(soundPlayer, _startLevel1Command);

            harderButton.Initialise(soundPlayer, this);
            hardButton.Initialise(soundPlayer, this);
            normalButton.Initialise(soundPlayer, this);
            easyButton.Initialise(soundPlayer, this);
        }

        private void StartLevel1()
        {
            Assert.IsTrue(CanStartLevel1());

            // Save difficulty
            Assert.IsTrue(_selectedDifficulty != null);
            Difficulty selectedDifficulty = (Difficulty)_selectedDifficulty;
            _settingsManager.AIDifficulty = selectedDifficulty;
            _settingsManager.Save();

            // Start level 1
            _screensSceneGod.GoToTrashScreen(levelNum: 1);
        }

        private bool CanStartLevel1()
        {
            return _selectedDifficulty != null;
        }

        public void ChooseDifficulty(Difficulty difficulty)
        {
            _selectedDifficulty = difficulty;
            _startLevel1Command.EmitCanExecuteChanged();
        }
    }
}
