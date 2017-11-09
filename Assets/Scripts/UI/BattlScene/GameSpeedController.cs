using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.UI;

namespace BattleCruisers.UI.BattleScene
{
    // TEMP:  For end game, allow chaging speed from settings OR simply lock game speed :)
    public class GameSpeedController : MonoBehaviour
    {
        private Text _gameSpeedText;

        private const float MAX_GAME_SPEED = 8;
        private const float MIN_GAME_SPEED = 0.125f;
        private const float SPEED_CHANGE_FACTOR = 2;

        private float GameSpeed
        {
            get { return Time.timeScale; }
            set
            {
                Time.timeScale = value;
                _gameSpeedText.text = value.ToString();
            }
        }

        void Start()
        {
            _gameSpeedText = GetComponent<Text>();
            Assert.IsNotNull(_gameSpeedText);
        }

        void Update()
        {
            // Increase game speed
            if (Input.GetKeyUp(KeyCode.Equals))
            {
                if (GameSpeed < MAX_GAME_SPEED)
                {
                    GameSpeed *= SPEED_CHANGE_FACTOR;
                }
            }
            // Reduce game speed
            else if (Input.GetKeyUp(KeyCode.Minus))
            {
                if (GameSpeed > MIN_GAME_SPEED)
                {
                    GameSpeed /= SPEED_CHANGE_FACTOR;
                }
            }
        }
    }
}
