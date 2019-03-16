using System.Collections;
using BattleCruisers.UI;
using BattleCruisers.UI.Music;
using BattleCruisers.UI.Sound;
using BattleCruisers.Utils;
using BattleCruisers.Utils.Fetchers;
using BattleCruisers.Utils.PlatformAbstractions.UI;
using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.SceneManagement;

namespace BattleCruisers.Scenes
{
    public class LandingSceneGod : MonoBehaviour, ISceneNavigator
    {
        private bool _isInitialised = false;

		public static ILoadingScreen LoadingScreen { get; private set; }
        public static ISceneNavigator SceneNavigator { get; private set; }
        public static IMusicPlayer MusicPlayer { get; private set; }

        void Start()
        {
            if (!_isInitialised)
            {
                LoadingScreenController loadingScreen = GetComponent<LoadingScreenController>();
                Assert.IsNotNull(loadingScreen);
                loadingScreen.Initialise();
                LoadingScreen = loadingScreen;

                // Persist this game object across scenes
                DontDestroyOnLoad(gameObject);
                _isInitialised = true;

                SceneNavigator = this;

                MusicPlayer = CreateMusicPlayer();

                // Game starts with the screens scene
                GoToScene(SceneNames.SCREENS_SCENE);
            }
        }

        private IMusicPlayer CreateMusicPlayer()
        {
            AudioSource platformAudioSource = GetComponent<AudioSource>();
            Assert.IsNotNull(platformAudioSource);
            IAudioSource audioSource = new AudioSourceBC(platformAudioSource);

            return
                new MusicPlayer(
                    new SingleSoundPlayer(
                        new SoundFetcher(),
                        audioSource));
        }

        public void GoToScene(string sceneName)
        {
            IEnumerator loadScene = LoadScene(sceneName);
            StartCoroutine(LoadingScreen.PerformLongOperation(loadScene));
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
			
            Logging.Log(Tags.SCENE_NAVIGATION, "Finished loading:  " + sceneName);
        }
    }
}
