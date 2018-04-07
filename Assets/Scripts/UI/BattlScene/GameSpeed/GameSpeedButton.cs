using System;
using UnityEngine;
using UnityEngine.Assertions;

namespace BattleCruisers.UI.BattleScene.GameSpeed
{
    public class GameSpeedButton : MonoBehaviour, IGameSpeedModifier
    {
        private const float MIN_GAME_SPEED = 0.25f;
        private const float MAX_GAME_SPEED = 4;

        public float gameSpeed;

        public event EventHandler<GameSpeedChangedEventArgs> GameSpeedChanged;

        void Start () 
        {
            Assert.IsTrue(gameSpeed >= MIN_GAME_SPEED);
            Assert.IsTrue(gameSpeed <= MAX_GAME_SPEED);
    	}

        public void ChangeGameSpeed()
        {
            Time.timeScale = gameSpeed;

            if (GameSpeedChanged != null)
            {
                GameSpeedChanged.Invoke(this, new GameSpeedChangedEventArgs(gameSpeed));
            }
        }
    }
}
