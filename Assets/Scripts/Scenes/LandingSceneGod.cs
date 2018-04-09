using System.Collections;
using BattleCruisers.UI;
using BattleCruisers.Utils;
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

        void Awake()
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

                // Game starts with the screens scene
                GoToScene(SceneNames.SCREENS_SCENE);
            }
        }

        public void GoToScene(string sceneName)
        {
            IEnumerator loadScene = LoadScene(sceneName);
            StartCoroutine(LoadingScreen.PerformLongOperation(loadScene));
        }

        private IEnumerator LoadScene(string sceneName)
        {
            Logging.Log(Tags.SCENE_NAVIGATION, "LoadScene():  Start loading:  " + sceneName);

            AsyncOperation loadingScene = SceneManager.LoadSceneAsync(sceneName, LoadSceneMode.Single); 

            while(!loadingScene.isDone)
            {
                Logging.Verbose(Tags.SCENE_NAVIGATION, "LoadScene():  " + sceneName + "  progress: " + loadingScene.progress);
                yield return null;
            }
			
            Logging.Log(Tags.SCENE_NAVIGATION, "LoadScene():  Finished loading:  " + sceneName);
        }
    }
}
