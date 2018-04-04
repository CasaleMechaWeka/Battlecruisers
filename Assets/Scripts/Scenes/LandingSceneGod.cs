using System.Collections;
using BattleCruisers.Utils;
using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace BattleCruisers.Scenes
{
    public class LandingSceneGod : MonoBehaviour, ISceneNavigator
    {
        private bool _isInitialised = false;

        // FELIX  Retrieve programmatically
        public Slider loadingProgress;
        public Canvas root;

        // An async operation is complete when its progres reaches 0.9.
        private const float MAX_PROGRESS = 0.9f;

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
                Helper.AssertIsNotNull(loadingProgress, root);

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
                float progress = AdjustPrgress(loadingScene.progress);
                loadingProgress.value = progress;
                Logging.Verbose(Tags.SCENE_NAVIGATION, "LoadScene():  " + sceneName + "  progress: " + progress);
                yield return null;
            }

            IsLoadingScene = false;
            Logging.Log(Tags.SCENE_NAVIGATION, "LoadScene():  Finished loading:  " + sceneName);
        }

        private float AdjustPrgress(float progress)
        {
            return Mathf.Clamp01(progress / MAX_PROGRESS);
        }
    }
}
