using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.UI;

namespace BattleCruisers.UI.BattleScene.GameSpeed
{
    public class GameSpeedButton : UIElement, IGameSpeedButton
    {
        private ISpeedButtonManager _speedButtonManager;
        private Image _backgroundImage;

        private static Color SELECTED_COLOR = Color.grey;
        private static Color DESELECTED_COLOR = Color.clear;

        private const float MIN_GAME_SPEED = 0.25f;
        private const float MAX_GAME_SPEED = 4;

        public float gameSpeed;

        public bool IsSelected
        {
            set
            {
                _backgroundImage.color = value ? SELECTED_COLOR : DESELECTED_COLOR;
            }
        }

        public void Initialise(ISpeedButtonManager speedButtonManager)
        {
            base.Initialise();

            Assert.IsNotNull(speedButtonManager);
			Assert.IsTrue(gameSpeed >= MIN_GAME_SPEED);
			Assert.IsTrue(gameSpeed <= MAX_GAME_SPEED);

            _speedButtonManager = speedButtonManager;

            _backgroundImage = GetComponent<Image>();
            Assert.IsNotNull(_backgroundImage);

            IsSelected = false;
        }

        public void ChangeGameSpeed()
        {
            Time.timeScale = gameSpeed;
            _speedButtonManager.SelectButton(this);
        }
    }
}
