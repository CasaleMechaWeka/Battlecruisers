using BattleCruisers.Data.Settings;
using BattleCruisers.Scenes;
using BattleCruisers.UI.Sound.Players;
using BattleCruisers.Utils;
using BattleCruisers.Utils.Localisation;

namespace BattleCruisers.UI.ScreensScene.ChooseDifficultyScreen
{
    public class ChooseDifficultyScreenController : ScreenController, IChooseDifficultyScreen
    {
        private ISettingsManager _settingsManager;

        public DifficultyButtonController harderButton, hardButton, normalButton, easyButton;

        public void Initialise(
            IScreensSceneGod screensSceneGod,
            ISingleSoundPlayer soundPlayer,
            ISettingsManager settingsManager,
            ILocTable screensSceneStrings)
        {
            base.Initialise(screensSceneGod);

            Helper.AssertIsNotNull(harderButton, hardButton, normalButton, easyButton);
            Helper.AssertIsNotNull(soundPlayer, settingsManager, screensSceneStrings);

            _settingsManager = settingsManager;

            harderButton.Initialise(soundPlayer, this, screensSceneStrings);
            hardButton.Initialise(soundPlayer, this, screensSceneStrings);
            normalButton.Initialise(soundPlayer, this, screensSceneStrings);
            easyButton.Initialise(soundPlayer, this, screensSceneStrings);
        }

        public void ChooseDifficulty(Difficulty difficulty)
        {
            _settingsManager.AIDifficulty = difficulty;
            _settingsManager.Save();

            _screensSceneGod.GoToTrashScreen(levelNum: 1);
        }
    }
}
