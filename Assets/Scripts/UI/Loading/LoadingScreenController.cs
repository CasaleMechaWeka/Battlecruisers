using BattleCruisers.Scenes;
using BattleCruisers.Utils;
using UnityEngine;
using UnityEngine.UI;

namespace BattleCruisers.UI.Loading
{
    public class LoadingScreenController : MonoBehaviour
    {
        public Canvas root;
        public Text loadingText;

        private const string DEFAULT_LOADING_TEXT = "Loading";

        void Start()
        {
            Helper.AssertIsNotNull(root, loadingText);

            loadingText.text = LandingSceneGod.LoadingScreenHint ?? DEFAULT_LOADING_TEXT;
            LandingSceneGod.MusicPlayer.PlayLoadingMusic();
        }
    }
}