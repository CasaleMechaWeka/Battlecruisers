using BattleCruisers.UI.Commands;
using BattleCruisers.UI.Common;
using BattleCruisers.UI.Sound.Players;
using BattleCruisers.Utils;
using NSubstitute;
using UnityCommon.PlatformAbstractions.Time;
using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.UI;

namespace BattleCruisers.UI.BattleScene.GameSpeed
{
    /// <summary>
    /// This class is only used in tests scenes :)
    /// </summary>
    public class GameSpeedController : MonoBehaviour
    {
        private ITime _time;
        private Text _gameSpeedText;
        private ICommand _increaseSpeedCommand, _decreaseSpeedCommand;
        private ButtonController _increaseSpeedButton, _decreaseSpeedButton;

        public float maxSpeed = DEFAULT_MAX_GAME_SPEED;
        public float minSpeed = DEFAULT_MIN_GAME_SPEED;
        public float initialGameSpeed = 1;

        // TEMP:  For end game, limit max speed to x4?
        private const float DEFAULT_MAX_GAME_SPEED = 4;
        //private const float MAX_GAME_SPEED = 32;
        private const float DEFAULT_MIN_GAME_SPEED = 0.125f;
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

        void Awake()
        {
            _time = TimeBC.Instance;

            _gameSpeedText = GetComponent<Text>();
            Assert.IsNotNull(_gameSpeedText);

            ISingleSoundPlayer soundPlayer = Substitute.For<ISingleSoundPlayer>();

            _increaseSpeedCommand = new Command(IncreaseSpeedCommandExecute, CanIncreaseSpeedCommandExecute);
            _increaseSpeedButton = transform.FindNamedComponent<TextGameSpeedButton>("IncreaseSpeedButton");
            _increaseSpeedButton.Initialise(soundPlayer, _increaseSpeedCommand);

            _decreaseSpeedCommand = new Command(DecreaseSpeedCommandExecute, CanDecreaseSpeedCommandExecute);
            _decreaseSpeedButton = transform.FindNamedComponent<TextGameSpeedButton>("DecreaseSpeedButton");
            _decreaseSpeedButton.Initialise(soundPlayer, _decreaseSpeedCommand);

            Assert.IsTrue(initialGameSpeed <= maxSpeed);
            Assert.IsTrue(initialGameSpeed >= minSpeed);
            GameSpeed = initialGameSpeed;
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
            return GameSpeed < maxSpeed;
        }

        private void IncreaseSpeedCommandExecute()
        {
            if (GameSpeed == 0)
            {
                GameSpeed = minSpeed;
            }
            else
            {
                GameSpeed *= SPEED_CHANGE_FACTOR;
            }
        }

        private bool CanDecreaseSpeedCommandExecute()
        {
            return GameSpeed > minSpeed;
        }

        private void DecreaseSpeedCommandExecute()
        {
            GameSpeed /= SPEED_CHANGE_FACTOR;
        }
    }
}
