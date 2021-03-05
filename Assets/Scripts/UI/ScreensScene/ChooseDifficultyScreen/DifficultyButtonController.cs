using BattleCruisers.Data.Settings;
using BattleCruisers.UI.Sound.Players;
using BattleCruisers.Utils;
using BattleCruisers.Utils.Localisation;
using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.UI;

namespace BattleCruisers.UI.ScreensScene.ChooseDifficultyScreen
{
    public class DifficultyButtonController : ElementWithClickSound
    {
        private IChooseDifficultyScreen _chooseDifficultyScreen;

        public Difficulty difficulty;

        public Text title, decription, aiDescription, aiBuildSpeed;
        public Image backgroundImage, stars;
        public Sprite defaultBackground, clickedBackground;
        public Color battlecruisersRed;
        public int buildSpeedPercentage = 125;

        public void Initialise(
            ISingleSoundPlayer soundPlayer,
            IChooseDifficultyScreen chooseDifficultyScreen,
            ILocTable screensSceneStrings)
        {
            base.Initialise(soundPlayer, parent: chooseDifficultyScreen);

            Helper.AssertIsNotNull(title, decription, aiDescription, aiBuildSpeed, backgroundImage, stars, defaultBackground, clickedBackground);
            Assert.IsNotNull(screensSceneStrings);

            string buildSpeedBase = screensSceneStrings.GetString("UI/DifficultyScreen/AIBuildSpeed");
            aiBuildSpeed.text = string.Format(buildSpeedBase, $"{buildSpeedPercentage}%");

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
            aiDescription.color = Color.white;
            aiBuildSpeed.color = Color.white;
        }

        protected override void ShowClickedState()
        {
            backgroundImage.sprite = clickedBackground;

            stars.color = battlecruisersRed;
            title.color = battlecruisersRed;
            decription.color = battlecruisersRed;
            aiDescription.color = battlecruisersRed;
            aiBuildSpeed.color = battlecruisersRed;
        }

        protected override void ShowHoverState()
        {
            ShowEnabledState();
            stars.color = Color.white;
        }
    }
}