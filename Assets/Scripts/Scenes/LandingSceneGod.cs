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

		public static ILoadingScreen LoadingScreen { get; private set; }
        public static ISceneNavigator SceneNavigator { get; private set; }
        public static IMusicPlayer MusicPlayer { get; private set; }

        void Start()
        {
            if (!_isInitialised)
            {
                IDataProvider dataProvider = ApplicationModelProvider.ApplicationModel.DataProvider;
                MusicPlayer = CreateMusicPlayer(dataProvider);

                LoadingScreenController loadingScreen = GetComponent<LoadingScreenController>();
                Assert.IsNotNull(loadingScreen);
                loadingScreen.Initialise(MusicPlayer);
                LoadingScreen = loadingScreen;

                // Persist this game object across scenes
                DontDestroyOnLoad(gameObject);
                _isInitialised = true;

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
            IEnumerator loadScene = LoadScene(sceneName);
            StartCoroutine(LoadingScreen.PerformLongOperation(loadScene, hint));
        }

        private IEnumerator LoadScene(string sceneName)
        {
            Logging.Log(Tags.SCENE_NAVIGATION, "Start loading:  " + sceneName);

            AsyncOperation loadingScene = SceneManager.LoadSceneAsync(sceneName, LoadSceneMode.Single); 

            while(!loadingScene.isDone)
            {
                Logging.Verbose(Tags.SCENE_NAVIGATION, $"{sceneName}  progress: {loadingScene.progress}");
                yield return null;
            }
			
            while (_lastSceneLoaded != sceneName)
            {
                float waitIntervalInS = 1;
                Logging.Verbose(Tags.SCENE_NAVIGATION, $"{sceneName}  waiting another: {waitIntervalInS}s");
                yield return new WaitForSeconds(waitIntervalInS);
            }

            Logging.Log(Tags.SCENE_NAVIGATION, "Finished loading:  " + sceneName);
        }

        public void SceneLoaded(string sceneName)
        {
            _lastSceneLoaded = sceneName;
        }
    }
}
