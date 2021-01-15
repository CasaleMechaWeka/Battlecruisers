using BattleCruisers.Data.Settings;
using BattleCruisers.Scenes;
using BattleCruisers.UI.Sound;
using BattleCruisers.Utils;

namespace BattleCruisers.UI.ScreensScene.ChooseDifficultyScreen
{
    public class ChooseDifficultyScreenController : ScreenController, IChooseDifficultyScreen
    {
        private ISettingsManager _settingsManager;

        public DifficultyButtonController harderButton, hardButton, normalButton, easyButton;

        public void Initialise(
            IScreensSceneGod screensSceneGod,
            ISingleSoundPlayer soundPlayer,
            ISettingsManager settingsManager)
        {
            base.Initialise(screensSceneGod);

            Helper.AssertIsNotNull(harderButton, hardButton, normalButton, easyButton);
            Helper.AssertIsNotNull(soundPlayer, settingsManager);

            _settingsManager = settingsManager;

            harderButton.Initialise(soundPlayer, this);
            hardButton.Initialise(soundPlayer, this);
            normalButton.Initialise(soundPlayer, this);
            easyButton.Initialise(soundPlayer, this);
        }

        public void ChooseDifficulty(Difficulty difficulty)
        {
            _settingsManager.AIDifficulty = difficulty;
            _settingsManager.Save();

            _screensSceneGod.GoToTrashScreen(levelNum: 1);
        }
    }
}
