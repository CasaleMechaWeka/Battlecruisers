using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Unity.Services.Authentication;
using BattleCruisers.Data;
using BattleCruisers.Data.Helpers;
using BattleCruisers.Data.Models;
using BattleCruisers.Data.Settings;
using BattleCruisers.Data.Static;
using BattleCruisers.UI;
using BattleCruisers.UI.Sound.Players;
using BattleCruisers.Utils.Fetchers;
using UnityEngine.Assertions;
using BattleCruisers.Utils.PlatformAbstractions.Audio;
using BattleCruisers.UI.Sound.AudioSources;

namespace BattleCruisers.Scenes
{
    public class CloudSaveLoadTestSceneGod : MonoBehaviour
    {
        private IApplicationModel _applicationModel;
        private IDataProvider _dataProvider;
        private IGameModel _gameModel;

        public Text idTextBox;
        public CanvasGroupButton saveBtn;

        void Start()
        {
            IApplicationModel applicationModel = ApplicationModelProvider.ApplicationModel;

            ISoundFetcher soundFetcher = new SoundFetcher();
            AudioSource platformAudioSource = GetComponent<AudioSource>();
            Assert.IsNotNull(platformAudioSource);
            IAudioSource audioSource
                = new MusicVolumeAudioSource(
                    new AudioSourceBC(platformAudioSource),
                    applicationModel.DataProvider.SettingsManager);

            ISingleSoundPlayer soundPlayer = new SingleSoundPlayer(
                new SoundFetcher(),
                audioSource
                );

            _applicationModel = ApplicationModelProvider.ApplicationModel;
            _dataProvider = _applicationModel.DataProvider;
            _gameModel = _dataProvider.GameModel;

            login();

            saveBtn.Initialise(soundPlayer, Save);
            
        }

        public async void Save()
        {
            await _dataProvider.CloudSave();
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
                }
                catch
                {
                    Debug.LogError("Login broke.");
                }
            }
        }
    }
}
