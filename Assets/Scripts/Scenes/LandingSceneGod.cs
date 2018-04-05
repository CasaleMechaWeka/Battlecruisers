using System.Collections;
using BattleCruisers.Utils;
using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.SceneManagement;

namespace BattleCruisers.Scenes
{
    public class LandingSceneGod : MonoBehaviour, ISceneNavigator
    {
        private bool _isInitialised = false;

        public Canvas root;

		private bool _isLoadingScene = false;
        private bool IsLoadingScene
        {
            get { return _isLoadingScene; }
            set
            {
                _isLoadingScene = value;
                root.gameObject.SetActive(_isLoadingScene);
            }
        }

        public static ISceneNavigator SceneNavigator { get; private set; }

        void Awake()
        {
            if (!_isInitialised)
            {
                Assert.IsNotNull(root);

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
            Assert.IsFalse(IsLoadingScene);

            IsLoadingScene = true;

            AsyncOperation loadingScene = SceneManager.LoadSceneAsync(sceneName, LoadSceneMode.Single); 

            while(!loadingScene.isDone)
            {
                Logging.Verbose(Tags.SCENE_NAVIGATION, "LoadScene():  " + sceneName + "  progress: " + loadingScene.progress);
                yield return null;
            }

            IsLoadingScene = false;
            Logging.Log(Tags.SCENE_NAVIGATION, "LoadScene():  Finished loading:  " + sceneName);
        }
    }
}
