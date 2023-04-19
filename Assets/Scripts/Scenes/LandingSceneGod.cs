
using BattleCruisers.Data;
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
using Unity.Services.Analytics;
using Unity.Services.Core;
using Unity.Services.Core.Environments;
using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace BattleCruisers.Scenes
{

    public class LandingSceneGod : MonoBehaviour, ISceneNavigator
    {
        public Text SubTitle;
        private bool _isInitialised = false;
        private string _lastSceneLoaded;
        private IHintProvider _hintProvider;

        [Header("For testing")]
        public bool testCreditsScene = false;

        [Header("For testing")]
        public bool testCutScene = false;

        public static ISceneNavigator SceneNavigator { get; private set; }
        public static IMusicPlayer MusicPlayer { get; private set; }
        public static string LoadingScreenHint { get; private set; }

        async void Start()
        {
            try
            {
                var options = new InitializationOptions();
                options.SetEnvironmentName("production");
                await UnityServices.InitializeAsync(options);
                List<string> consentIdentifiers = await AnalyticsService.Instance.CheckForRequiredConsents();
            }
            catch (ConsentCheckException e)
            {
                //do nothing
                Debug.Log(e.Message);
            }

            IApplicationModel applicationModel = ApplicationModelProvider.ApplicationModel;


            ILocTable commonStrings = await LocTableFactory.Instance.LoadCommonTableAsync();
            string subTitle = commonStrings.GetString("GameNameSubtitle").ToUpper();

#if FREE_EDITION || UNITY_EDITOR
            //if player NOT already paid then use Free title
            if (!applicationModel.DataProvider.GameModel.PremiumEdition)
                subTitle = commonStrings.GetString("GameNameFreeEdition").ToUpper();
#else
            //if premium version set here 
            applicationModel.DataProvider.GameModel.PremiumEdition = true;
            applicationModel.DataProvider.SaveGame();
#endif

            SubTitle.text = subTitle;

            Logging.Log(Tags.SCENE_NAVIGATION, $"_isInitialised: {_isInitialised}");

            if (!_isInitialised)
            {
                IDataProvider dataProvider = ApplicationModelProvider.ApplicationModel.DataProvider;
                MusicPlayer = CreateMusicPlayer(dataProvider);

                if (!dataProvider.GameModel.Settings.InitialisedGraphics)
                {
                    dataProvider.GameModel.Settings.InitialiseGraphicsSettings();
                }
                Screen.SetResolution(Math.Max(600, dataProvider.GameModel.Settings.ResolutionWidth), Math.Max(400, dataProvider.GameModel.Settings.ResolutionHeight - (dataProvider.GameModel.Settings.FullScreen ? 0 : (int)(dataProvider.GameModel.Settings.ResolutionHeight * 0.06))), dataProvider.GameModel.Settings.FullScreen ? (FullScreenMode)1 : (FullScreenMode)3);
                //Screen.SetResolution(Screen.currentResolution.width, Screen.currentResolution.height , dataProvider.GameModel.Settings.FullScreen ? (FullScreenMode)1 : (FullScreenMode)3);
                // Persist this game object across scenes
                DontDestroyOnLoad(gameObject);
                _isInitialised = true;

                SceneNavigator = this;

                HintProviders hintProviders = new HintProviders(RandomGenerator.Instance, commonStrings);
                _hintProvider = new CompositeHintProvider(hintProviders.BasicHints, hintProviders.AdvancedHints, dataProvider.GameModel, RandomGenerator.Instance);

                //Debug.Log(Screen.currentResolution);
                // Game starts with the screens scene
                if (testCreditsScene)
                {
                    GoToScene(SceneNames.CREDITS_SCENE, true);
                }
                else if (testCutScene)
                {
                    GoToScene(SceneNames.CUTSCENE_SCENE, true);
                }
                else
                {
                    GoToScene(SceneNames.SCREENS_SCENE, true);
                }
            }
        }



        private IMusicPlayer CreateMusicPlayer(IDataProvider dataProvider)
        {
            AudioSource platformAudioSource = GetComponent<AudioSource>();
            Assert.IsNotNull(platformAudioSource);
            IAudioSource audioSource
                = new MusicVolumeAudioSource(
                    new AudioSourceBC(platformAudioSource),
                    dataProvider.SettingsManager);

            return
                new MusicPlayer(
                    new SingleSoundPlayer(
                        new SoundFetcher(),
                        audioSource));
        }

        public void GoToScene(string sceneName, bool stopMusic)
        {
            string hint = null;
            if (sceneName == SceneNames.BATTLE_SCENE
                && !ApplicationModelProvider.ApplicationModel.IsTutorial)
            {
                hint = _hintProvider.GetHint();
            }
            LoadingScreenHint = hint;

            if (stopMusic)
                MusicPlayer.Stop();

            StartCoroutine(LoadSceneWithLoadingScreen(sceneName));
        }

        private IEnumerator LoadSceneWithLoadingScreen(string sceneName)
        {
            Logging.LogMethod(Tags.SCENE_NAVIGATION);

            _lastSceneLoaded = null;
            if (sceneName == SceneNames.MULTIPLAY_SCREENS_SCENE)
                yield return LoadScene(SceneNames.MULTIPLAY_STARTUP_SCENE, LoadSceneMode.Single);
            else
                yield return LoadScene(SceneNames.LOADING_SCENE, LoadSceneMode.Single);
            yield return LoadScene(sceneName, LoadSceneMode.Additive);

            Logging.Log(Tags.SCENE_NAVIGATION, "Wait for my custom setup for:  " + sceneName);

            while (_lastSceneLoaded != sceneName)
            {
                float waitIntervalInS = 0.1f;
                Logging.Verbose(Tags.SCENE_NAVIGATION, $"Loading {sceneName}  waiting another: {waitIntervalInS}s");
                yield return new WaitForSeconds(waitIntervalInS);
            }
            Logging.Log(Tags.SCENE_NAVIGATION, "Finished loading:  " + sceneName);

            // Hide loading scene.  Don't unload, because that destroys all prefabs that have been loaded :P
            LoadingScreenController.Instance.Destroy();
        }

        private IEnumerator LoadScene(string sceneName, LoadSceneMode loadSceneMode)
        {
            Logging.Log(Tags.SCENE_NAVIGATION, "Start loading:  " + sceneName);
            AsyncOperation loadOperation = SceneManager.LoadSceneAsync(sceneName, loadSceneMode);

            while (!loadOperation.isDone)
            {
                Logging.Verbose(Tags.SCENE_NAVIGATION, $"Loading {sceneName}  progress: {loadOperation.progress}");
                yield return null;
            }

            Logging.Log(Tags.SCENE_NAVIGATION, "Finished loading:  " + sceneName);
        }

        public void SceneLoaded(string sceneName)
        {
            Logging.Log(Tags.SCENE_NAVIGATION, sceneName);
            _lastSceneLoaded = sceneName;
        }

        void update()
        {
            transform.localPosition = Camera.main.gameObject.transform.localPosition;
        }

    }
}
