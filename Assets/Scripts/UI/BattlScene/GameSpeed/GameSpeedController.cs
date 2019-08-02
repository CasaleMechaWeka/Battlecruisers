using BattleCruisers.UI.Commands;
using BattleCruisers.UI.Common;
using BattleCruisers.Utils;
using UnityCommon.PlatformAbstractions;
using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.UI;

namespace BattleCruisers.UI.BattleScene.GameSpeed
{
    public class GameSpeedController : MonoBehaviour
    {
        private ITime _time;
        private Text _gameSpeedText;
        private ICommand _increaseSpeedCommand, _decreaseSpeedCommand;
        private ButtonController _increaseSpeedButton, _decreaseSpeedButton;

        // TEMP:  For end game, limit max speed to x4?
        private const float MAX_GAME_SPEED = 4;
        //private const float MAX_GAME_SPEED = 32;
        private const float MIN_GAME_SPEED = 0.125f;
        private const float SPEED_CHANGE_FACTOR = 2;

        private const string SPEED_PREFIX = "x";

        private float GameSpeed
        {
            get { return _time.TimeScale; }
            set
            {
                _time.TimeScale = value;
                _gameSpeedText.text = SPEED_PREFIX + value.ToString();

                _increaseSpeedCommand.EmitCanExecuteChanged();
                _decreaseSpeedCommand.EmitCanExecuteChanged();
            }
        }

        void Start()
        {
            _time = new TimeBC();

            _gameSpeedText = GetComponent<Text>();
            Assert.IsNotNull(_gameSpeedText);

            _increaseSpeedCommand = new Command(IncreaseSpeedCommandExecute, CanIncreaseSpeedCommandExecute);
            _increaseSpeedButton = transform.FindNamedComponent<TextGameSpeedButton>("IncreaseSpeedButton");
            _increaseSpeedButton.Initialise(_increaseSpeedCommand);

            _decreaseSpeedCommand = new Command(DecreaseSpeedCommandExecute, CanDecreaseSpeedCommandExecute);
            _decreaseSpeedButton = transform.FindNamedComponent<TextGameSpeedButton>("DecreaseSpeedButton");
            _decreaseSpeedButton.Initialise(_decreaseSpeedCommand);
        }

        void Update()
        {
            // Increase game speed
            if (Input.GetKeyUp(KeyCode.Equals))
            {
                _increaseSpeedCommand.ExecuteIfPossible();
            }
            // Reduce game speed
            else if (Input.GetKeyUp(KeyCode.Minus))
            {
                _decreaseSpeedCommand.ExecuteIfPossible();
            }
        }

        private bool CanIncreaseSpeedCommandExecute()
        {
            return GameSpeed < MAX_GAME_SPEED;
        }

        private void IncreaseSpeedCommandExecute()
        {
            GameSpeed *= SPEED_CHANGE_FACTOR;
        }

        private bool CanDecreaseSpeedCommandExecute()
        {
            return GameSpeed > MIN_GAME_SPEED;
        }

        private void DecreaseSpeedCommandExecute()
        {
            GameSpeed /= SPEED_CHANGE_FACTOR;
        }
    }
}
