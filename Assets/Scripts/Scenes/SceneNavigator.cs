using System.Threading.Tasks;
using BattleCruisers.Data;
using BattleCruisers.UI.Loading;
using BattleCruisers.UI.ScreensScene.Multiplay.ArenaScreen;
using BattleCruisers.Utils;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace BattleCruisers.Scenes
{
    public static class SceneNavigator
    {
        public static string LoadingScreenHint = null;
        private static string _lastSceneLoaded;

        public static void GoToScene(string sceneName, bool stopMusic)
        {
            string hint = null;
            if (sceneName == SceneNames.BATTLE_SCENE
                && !ApplicationModel.IsTutorial)
            {
                hint = HintProvider.GetHint();
            }
            if (sceneName == SceneNames.PvP_BOOT_SCENE && !ApplicationModel.IsTutorial)
            {
                // should be replace with PvP
                hint = HintProvider.GetHint();
            }

            LoadingScreenHint = hint;

            if (LandingSceneGod.MusicPlayer != null && stopMusic)
                LandingSceneGod.MusicPlayer.Stop();

            _ = LoadSceneWithLoadingScreen(sceneName);
        }

        private static async Task LoadSceneWithLoadingScreen(string sceneName)
        {
            Logging.LogMethod(Tags.SCENE_NAVIGATION);

            _lastSceneLoaded = null;
            if (sceneName == SceneNames.PvP_BOOT_SCENE)
            {
                await LoadSceneAsync(SceneNames.PvP_INITIALIZE_SCENE, LoadSceneMode.Single);
            }
            else
            {
                await LoadSceneAsync(SceneNames.LOADING_SCENE, LoadSceneMode.Single);
            }

            await LoadSceneAsync(sceneName, LoadSceneMode.Additive);

            Logging.Log(Tags.SCENE_NAVIGATION, "Wait for my custom setup for:  " + sceneName);

            while (_lastSceneLoaded != sceneName)
            {
                const int waitIntervalInMs = 100;
                Logging.Verbose(Tags.SCENE_NAVIGATION, $"Loading {sceneName}  waiting another: {waitIntervalInMs}s");
                await Task.Delay(waitIntervalInMs);
            }
            Logging.Log(Tags.SCENE_NAVIGATION, "Finished loading:  " + sceneName);

            // Hide loading scene.  Don't unload, because that destroys all prefabs that have been loaded :P

            if (sceneName == SceneNames.PvP_BOOT_SCENE)
            {
                if (MatchmakingScreenController.Instance != null)
                    MatchmakingScreenController.Instance.Destroy();
            }
            else
            {
                if (LoadingScreenController.Instance != null)
                    LoadingScreenController.Instance.Destroy();
            }
        }

        private static async Task LoadSceneAsync(string sceneName, LoadSceneMode loadSceneMode)
        {
            Logging.Log(Tags.SCENE_NAVIGATION, "Start loading:  " + sceneName);
            AsyncOperation loadOperation = SceneManager.LoadSceneAsync(sceneName, loadSceneMode);

            while (!loadOperation.isDone)
            {
                Logging.Verbose(Tags.SCENE_NAVIGATION, $"Loading {sceneName}  progress: {loadOperation.progress}");
                await Task.Yield();
            }

            Logging.Log(Tags.SCENE_NAVIGATION, "Finished loading:  " + sceneName);
        }

        public static void SceneLoaded(string sceneName)
        {
            Logging.Log(Tags.SCENE_NAVIGATION, sceneName);
            _lastSceneLoaded = sceneName;
        }
        public static async Task GoToSceneAsync(string sceneName, bool stopMusic)
        {
            string hint = null;
            if (sceneName == SceneNames.BATTLE_SCENE && !ApplicationModel.IsTutorial)
                hint = HintProvider.GetHint();
            if (sceneName == SceneNames.PvP_BOOT_SCENE && !ApplicationModel.IsTutorial)
                hint = HintProvider.GetHint();

            LoadingScreenHint = hint;

            if (LandingSceneGod.MusicPlayer != null && stopMusic)
                LandingSceneGod.MusicPlayer.Stop();

            await LoadSceneWithLoadingScreen(sceneName);
        }
    }
}
