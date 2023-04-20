using BattleCruisers.Data.Helpers;
using BattleCruisers.Data.Static;
using BattleCruisers.Scenes;
using BattleCruisers.UI.Sound;
using BattleCruisers.UI.Sound.Players;
using System.Threading.Tasks;

namespace BattleCruisers.UI.ScreensScene.LevelsScreen
{
    public class SecretLevelButtonController : ElementWithClickSound
    {
        private IScreensSceneGod _screensSceneGod;
        private LevelInfo _level;

        protected override ISoundKey ClickSound => SoundKeys.UI.Click;

        public async Task InitialiseAsync(LevelInfo level, IScreensSceneGod screensSceneGod, ISingleSoundPlayer soundPlayer)
        {
            _level = level;
            _screensSceneGod = screensSceneGod;

            // Call the base class Initialise method
            Initialise(soundPlayer);
        }

        protected override void OnClicked()
        {
            base.OnClicked();
            _screensSceneGod.GoToTrashScreen(_level.Num);
        }
    }
}
