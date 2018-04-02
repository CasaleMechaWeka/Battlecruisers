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

        // An asyn operation is complete when its progres reaches 0.9.
        private const float MAX_PROGRESS = 0.9f;

		private bool _isLoadingScene = false;
        private bool IsLoadingScene
        {
            get { return _isLoadingScene; }
            set
            {
                _isLoadingScene = value;
                loadingProgress.gameObject.SetActive(_isLoadingScene);
            }
        }

        void Awake()
        {
            if (!_isInitialised)
            {
                Assert.IsNotNull(loadingProgress);

                // Persist this game object across scenes
                DontDestroyOnLoad(gameObject);
                _isInitialised = true;

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
            Assert.IsFalse(IsLoadingScene);

            IsLoadingScene = true;
            
            AsyncOperation loadingScene = SceneManager.LoadSceneAsync(sceneName, LoadSceneMode.Single); 

            while(!loadingScene.isDone)
            {
                float progress = AdjustPrgress(loadingScene.progress);
                loadingProgress.value = progress;
                yield return null;
            }

            IsLoadingScene = false;
        }

        private float AdjustPrgress(float progress)
        {
            return Mathf.Clamp01(progress / MAX_PROGRESS);
        }
    }
}
