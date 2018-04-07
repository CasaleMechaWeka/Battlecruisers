using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;

namespace BattleCruisers.UI.BattleScene
{
    public class StaticGameSpeedController : MonoBehaviour 
    {
        private const float MIN_GAME_SPEED = 0.25f;
        private const float MAX_GAME_SPEED = 4;

        public float gameSpeed;

    	void Start () 
        {
            Assert.IsTrue(gameSpeed >= MIN_GAME_SPEED);
            Assert.IsTrue(gameSpeed <= MAX_GAME_SPEED);
    	}

        public void ChangeGameSpeed()
        {
            Time.timeScale = gameSpeed;
        }
    }
}
