using BattleCruisers.Data.Settings;
using BattleCruisers.Scenes;
using BattleCruisers.UI.Sound;
using BattleCruisers.Utils;
using UnityEngine.Assertions;

namespace BattleCruisers.UI.ScreensScene.ChooseDifficultyScreen
{
    public class ChooseDifficultyScreenController : ScreenController, IChooseDifficultyScreen
    {
        private ISettingsManager _settingsManager;
        private Difficulty? _selectedDifficulty;

        public DifficultyButtonController harderButton, hardButton, normalButton, easyButton;

        // FELIX  Doesn't have to be async? :P
        public void Initialise(
            IScreensSceneGod screensSceneGod,
            ISingleSoundPlayer soundPlayer,
            ISettingsManager settingsManager)
        {
            base.Initialise(screensSceneGod);

            Helper.AssertIsNotNull(harderButton, hardButton, normalButton, easyButton);
            Helper.AssertIsNotNull(soundPlayer, settingsManager);

            _settingsManager = settingsManager;
            _selectedDifficulty = null;

            harderButton.Initialise(soundPlayer, this);
            hardButton.Initialise(soundPlayer, this);
            normalButton.Initialise(soundPlayer, this);
            easyButton.Initialise(soundPlayer, this);
        }

        public void ChooseDifficulty(Difficulty difficulty)
        {
            _selectedDifficulty = difficulty;

            // FELIX  Activate start level button
        }

        public void StartLevel1()
        {
            // Save difficulty
            Assert.IsTrue(_selectedDifficulty != null);
            Difficulty selectedDifficulty = (Difficulty)_selectedDifficulty;
            _settingsManager.AIDifficulty = selectedDifficulty;
            _settingsManager.Save();

            // Start level 1
            _screensSceneGod.GoToTrashScreen(levelNum: 1);
        }
    }
}
