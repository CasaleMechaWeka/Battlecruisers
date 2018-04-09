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
        private ILoadingScreen _loadingScreen;

        public static ISceneNavigator SceneNavigator { get; private set; }

        void Awake()
        {
            if (!_isInitialised)
            {
                _loadingScreen = GetComponentInChildren<ILoadingScreen>();
                Assert.IsNotNull(_loadingScreen);

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
            StartCoroutine(LoadScene(sceneName));
        }

        private IEnumerator LoadScene(string sceneName)
        {
            Logging.Log(Tags.SCENE_NAVIGATION, "LoadScene():  Start loading:  " + sceneName);
            _loadingScreen.IsVisible = true;

            AsyncOperation loadingScene = SceneManager.LoadSceneAsync(sceneName, LoadSceneMode.Single); 

            while(!loadingScene.isDone)
            {
                Logging.Verbose(Tags.SCENE_NAVIGATION, "LoadScene():  " + sceneName + "  progress: " + loadingScene.progress);
                yield return null;
            }

            _loadingScreen.IsVisible = false;
            Logging.Log(Tags.SCENE_NAVIGATION, "LoadScene():  Finished loading:  " + sceneName);
        }
    }
}
