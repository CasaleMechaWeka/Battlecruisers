using BattleCruisers.Scenes;
using UnityEngine;
using UnityEngine.UI;

namespace BattleCruisers.UI.ScreensScene.LevelsScreen
{
    public class SecretLevelButtonController : MonoBehaviour
    {
        [SerializeField] private int secretLevelNum;
        [SerializeField] private Button button;

        private IScreensSceneGod _screensSceneGod;

        public void Initialise(IScreensSceneGod screensSceneGod)
        {
            _screensSceneGod = screensSceneGod;

            button.onClick.AddListener(() =>
            {
                if (_screensSceneGod != null)
                {
                    _screensSceneGod.GoToTrashScreen(secretLevelNum);
                }
                else
                {
                    Debug.LogError("ScreensSceneGod is not initialized");
                }
            });
        }
    }
}
