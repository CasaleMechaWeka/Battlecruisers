using UnityEngine;
using UnityEngine.UI;
using Unity.Services.Authentication;
using BattleCruisers.Data;
using BattleCruisers.Data.Models;
using BattleCruisers.UI;
using BattleCruisers.UI.Sound.Players;
using UnityEngine.Assertions;
using BattleCruisers.Utils.PlatformAbstractions.Audio;
using BattleCruisers.UI.Sound.AudioSources;

namespace BattleCruisers.Scenes
{
    public class CloudSaveLoadTestSceneGod : MonoBehaviour
    {
        private IApplicationModel _applicationModel;
        private DataProvider _dataProvider;
        private IGameModel _gameModel;

        public InputField userIdInputField;
        public Text idTextBox;
        public Text coinsTextBox;
        public Text creditsTextBox;
        public CanvasGroupButton saveBtn, loadBtn;

        void Start()
        {
            IApplicationModel applicationModel = ApplicationModelProvider.ApplicationModel;

            AudioSource platformAudioSource = GetComponent<AudioSource>();
            Assert.IsNotNull(platformAudioSource);
            IAudioSource audioSource
                = new MusicVolumeAudioSource(
                    new AudioSourceBC(platformAudioSource),
                    applicationModel.DataProvider.SettingsManager);

            ISingleSoundPlayer soundPlayer = new SingleSoundPlayer(
                audioSource
                );

            _applicationModel = ApplicationModelProvider.ApplicationModel;
            _dataProvider = _applicationModel.DataProvider;
            _gameModel = _dataProvider.GameModel;

            login();

            saveBtn.Initialise(soundPlayer, Save);
            loadBtn.Initialise(soundPlayer, Load);
        }

        public async void Save()
        {
            await _dataProvider.CloudSave();
        }

        public async void Load()
        {
            await _dataProvider.CloudLoad();
            // Adding logs to verify data load
            Debug.Log($"Loaded Coins: {_gameModel.Coins}, Credits: {_gameModel.Credits}");
            DisplayCurrency();
        }

        public async void login()
        {
            if (!AuthenticationService.Instance.IsSignedIn)
            {
                try
                {
                    await AuthenticationService.Instance.SignInAnonymouslyAsync();
                    idTextBox.text = AuthenticationService.Instance.PlayerId;
                    Debug.Log("=====> PlayerInfo --->" + AuthenticationService.Instance.PlayerId);
                    // Load player data after login
                    await _dataProvider.CloudLoad();
                    DisplayCurrency();
                }
                catch
                {
                    Debug.LogError("Login broke.");
                }
            }
        }

        private void DisplayCurrency()
        {
            // Update the UI with the player's coins and credits
            coinsTextBox.text = "Coins: " + _gameModel.Coins;
            creditsTextBox.text = "Credits: " + _gameModel.Credits;
        }
    }
}
