using BattleCruisers.Data.Settings;
using BattleCruisers.UI.Sound.Players;
using BattleCruisers.Utils;
using UnityEngine;
using UnityEngine.UI;

namespace BattleCruisers.UI.ScreensScene.ChooseDifficultyScreen
{
    public class DifficultyButtonController : ElementWithClickSound
    {
        private IChooseDifficultyScreen _chooseDifficultyScreen;

        public Difficulty difficulty;

        public Text title, decription, modifications;
        public Image backgroundImage, stars;
        public Sprite defaultBackground, clickedBackground;
        public Color battlecruisersRed;

        public void Initialise(
            ISingleSoundPlayer soundPlayer,
            IChooseDifficultyScreen chooseDifficultyScreen)
        {
            base.Initialise(soundPlayer, parent: chooseDifficultyScreen);

            Helper.AssertIsNotNull(title, decription, modifications, backgroundImage, stars, defaultBackground, clickedBackground);

            _chooseDifficultyScreen = chooseDifficultyScreen;
        }

        protected override void OnClicked()
        {
            base.OnClicked();
            _chooseDifficultyScreen.ChooseDifficulty(difficulty);
        }

        protected override void ShowEnabledState()
        {
            backgroundImage.sprite = defaultBackground;

            stars.color = Color.black;
            title.color = Color.white;
            decription.color = Color.white;
            modifications.color = Color.white;
        }

        protected override void ShowClickedState()
        {
            backgroundImage.sprite = clickedBackground;

            stars.color = battlecruisersRed;
            title.color = battlecruisersRed;
            decription.color = battlecruisersRed;
            modifications.color = battlecruisersRed;
        }

        protected override void ShowHoverState()
        {
            ShowEnabledState();
            stars.color = Color.white;
        }
    }
}