using BattleCruisers.Data;
using BattleCruisers.UI.Loading;
using BattleCruisers.UI.Music;
using BattleCruisers.UI.Sound;
using BattleCruisers.Utils;
using BattleCruisers.Utils.Fetchers;
using BattleCruisers.Utils.PlatformAbstractions.Audio;
using System.Collections;
using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.SceneManagement;

namespace BattleCruisers.Scenes
{
    public class LandingSceneGod : MonoBehaviour, ISceneNavigator
    {
        private bool _isInitialised = false;
        private string _lastSceneLoaded;
        private IHintProvider _hintProvider;

        [Header("For testing")]
        public bool testCreditsScene = false;

        public static ISceneNavigator SceneNavigator { get; private set; }
        public static IMusicPlayer MusicPlayer { get; private set; }
        public static string LoadingScreenHint { get; private set; }

        void Start()
        {
            Logging.Log(Tags.SCENE_NAVIGATION, $"_isInitialised: {_isInitialised}");

            if (!_isInitialised)
            {
                IDataProvider dataProvider = ApplicationModelProvider.ApplicationModel.DataProvider;
                MusicPlayer = CreateMusicPlayer(dataProvider);

                // Persist this game object across scenes
                DontDestroyOnLoad(gameObject);
                _isInitialised = true;
                _lastSceneLoaded = null;

                SceneNavigator = this;

                HintProviders hintProviders = new HintProviders(RandomGenerator.Instance);
                _hintProvider = new CompositeHintProvider(hintProviders.BasicHints, hintProviders.AdvancedHints, dataProvider.GameModel, RandomGenerator.Instance);

                // Game starts with the screens scene
                if (testCreditsScene)
                {
                    GoToScene(SceneNames.CREDITS_SCENE);
                }
                else
                {
                    GoToScene(SceneNames.SCREENS_SCENE);
                }
            }
        }

        private IMusicPlayer CreateMusicPlayer(IDataProvider dataProvider)
        {
            AudioSource platformAudioSource = GetComponent<AudioSource>();
            Assert.IsNotNull(platformAudioSource);
            IAudioSource audioSource = new AudioSourceBC(platformAudioSource);

            IMusicPlayer corePlayer
                = new MusicPlayer(
                    new SingleSoundPlayer(
                        new SoundFetcher(),
                        audioSource));
            return
                new TogglableMusicPlayer(
                    corePlayer,
                    dataProvider.SettingsManager);
        }

        public void GoToScene(string sceneName)
        {
            string hint = null;
            if (sceneName == SceneNames.BATTLE_SCENE
                && !ApplicationModelProvider.ApplicationModel.IsTutorial)
            {
                hint = _hintProvider.GetHint();
            }
            LoadingScreenHint = hint;

            StartCoroutine(LoadScene(sceneName));
        }

        private IEnumerator LoadScene(string sceneName)
        {
            Logging.LogMethod(Tags.SCENE_NAVIGATION);
            Assert.IsNotNull(sceneName);
            Assert.AreNotEqual(sceneName, _lastSceneLoaded);

            // Show loading scene
            // FELIX  Extract to avoid duplication with below :)
            Logging.Log(Tags.SCENE_NAVIGATION, "Start loading:  " + SceneNames.LOADING_SCENE);
            AsyncOperation showLoadingScene = SceneManager.LoadSceneAsync(SceneNames.LOADING_SCENE, LoadSceneMode.Single);

            while (!showLoadingScene.isDone)
            {
                Logging.Verbose(Tags.SCENE_NAVIGATION, $"Loading {SceneNames.LOADING_SCENE}  progress: {showLoadingScene.progress}");
                yield return null;
            }
            Logging.Log(Tags.SCENE_NAVIGATION, "Finished loading:  " + SceneNames.LOADING_SCENE);

            // FELIX  Extract
            Logging.Log(Tags.SCENE_NAVIGATION, "Start loading:  " + sceneName);
            AsyncOperation loadingOperation = SceneManager.LoadSceneAsync(sceneName, LoadSceneMode.Additive); 

            // Unity loading scene
            while(!loadingOperation.isDone)
            {
                Logging.Verbose(Tags.SCENE_NAVIGATION, $"Loading {sceneName}  progress: {loadingOperation.progress}");
                yield return null;
            }

            // My custom setup in the scene's Start() method
            Logging.Log(Tags.SCENE_NAVIGATION, "Wait for my custom setup for:  " + sceneName);

            while (_lastSceneLoaded != sceneName)
            {
                float waitIntervalInS = 0.1f;
                Logging.Verbose(Tags.SCENE_NAVIGATION, $"Loading {sceneName}  waiting another: {waitIntervalInS}s");
                yield return new WaitForSeconds(waitIntervalInS);
            }
            Logging.Log(Tags.SCENE_NAVIGATION, "Finished loading:  " + sceneName);

            // Hide loading scene.  Don't unload, because the destroys all prefabs that have been loaded :P
            LoadingScreenController.Instance.Destroy();
        }

        public void SceneLoaded(string sceneName)
        {
            _lastSceneLoaded = sceneName;
        }
    }
}
