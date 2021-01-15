using BattleCruisers.Data.Settings;
using BattleCruisers.UI.Sound;

namespace BattleCruisers.UI.ScreensScene.ChooseDifficultyScreen
{
    public class DifficultyButtonController : ElementWithClickSound
    {
        private IChooseDifficultyScreen _chooseDifficultyScreen;

        public Difficulty difficulty;

        public void Initialise(
            ISingleSoundPlayer soundPlayer,
            IChooseDifficultyScreen chooseDifficultyScreen)
        {
            base.Initialise(soundPlayer, parent: chooseDifficultyScreen);

            _chooseDifficultyScreen = chooseDifficultyScreen;
        }

        protected override void OnClicked()
        {
            base.OnClicked();
            _chooseDifficultyScreen.ChooseDifficulty(difficulty);
        }
    }
}