using BattleCruisers.Buildables;
using BattleCruisers.Data;
using BattleCruisers.Data.Models;
using BattleCruisers.Data.Models.PrefabKeys;
using BattleCruisers.PostBattleScreen;
using BattleCruisers.Scenes.BattleScene;
using BattleCruisers.UI;
using BattleCruisers.UI.Loading;
using BattleCruisers.UI.Music;
using BattleCruisers.UI.Sound.AudioSources;
using BattleCruisers.UI.Sound.Players;
using BattleCruisers.Utils;
using BattleCruisers.Utils.Fetchers;
using BattleCruisers.Utils.Localisation;
using BattleCruisers.Utils.PlatformAbstractions.Audio;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace BattleCruisers.Scenes
{
    public class InterstitialSceneGod : MonoBehaviour
    {
        private ISceneNavigator _sceneNavigator;
        public CanvasGroupButton nextButton;
        [SerializeField]
        private AudioSource _uiAudioSource;
        private ISingleSoundPlayer _soundPlayer;
        public GameObject[] screens;
        [Header("The number of the screen that comes up when running this scene.")]
        public int TestingScreen = 1;
        async void Start()
        {
            _sceneNavigator = LandingSceneGod.SceneNavigator;
            
            IGameModel gm = ApplicationModelProvider.ApplicationModel.DataProvider.GameModel;
            if (_sceneNavigator == null)
            {
                screens[TestingScreen-1].SetActive(true);
            }
            else{
                for (int i = 0; i < LevelStages.STAGE_STARTS.Length; i++)
                {
                    screens[i].SetActive(false);
                    if (gm.SelectedLevel-1 == LevelStages.STAGE_STARTS[i])
                    {
                        screens[i].SetActive(true);
                    }
                }
            }
            
            
            
            //LandingSceneGod.MusicPlayer.PlayVictoryMusic();

            _soundPlayer
                = new SingleSoundPlayer(
                    new SoundFetcher(),
                    new EffectVolumeAudioSource(
                        new AudioSourceBC(_uiAudioSource),
                        ApplicationModelProvider.ApplicationModel.DataProvider.SettingsManager, 1));


            nextButton.Initialise(_soundPlayer, Done);
            _sceneNavigator.SceneLoaded(SceneNames.STAGE_INTERSTITIAL_SCENE);
        }

        void Update()
        {
            if (Input.GetKeyUp(KeyCode.Escape)
                || Input.GetKeyUp(KeyCode.Space)
                || Input.GetKeyUp(KeyCode.Return))
            {
                Done();
            }
        }

        private void Done()
        {
            _sceneNavigator.GoToScene(SceneNames.SCREENS_SCENE, true);
        }
    }
}
