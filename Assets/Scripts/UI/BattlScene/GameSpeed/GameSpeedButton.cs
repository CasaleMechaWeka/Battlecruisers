using UnityEngine;
using UnityEngine.Assertions;

namespace BattleCruisers.UI.BattleScene.GameSpeed
{
    public class GameSpeedButton : MonoBehaviour, IGameSpeedButton
    {
        private ISpeedButtonManager _speedButtonManager;

        private const float MIN_GAME_SPEED = 0.25f;
        private const float MAX_GAME_SPEED = 4;

        public float gameSpeed;

        public bool IsSelected
        {
            set
            {
				// FELIX  Show UI effect :P
            }
        }

        public void Initialise(ISpeedButtonManager speedButtonManager)
        {
            Assert.IsNotNull(speedButtonManager);
			Assert.IsTrue(gameSpeed >= MIN_GAME_SPEED);
			Assert.IsTrue(gameSpeed <= MAX_GAME_SPEED);

            _speedButtonManager = speedButtonManager;
        }

        public void ChangeGameSpeed()
        {
            Time.timeScale = gameSpeed;
            _speedButtonManager.SelectButton(this);
        }
    }
}
