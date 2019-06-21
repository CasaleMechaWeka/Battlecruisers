using BattleCruisers.UI.Music;
using BattleCruisers.Utils;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

namespace BattleCruisers.UI.Loading
{
    public class LoadingScreenController : MonoBehaviour, ILoadingScreen
    {
        private IMusicPlayer _musicPlayer;

        public Canvas root;
        public Text loadingText;

        private const string DEFAULT_LOADING_TEXT = "Loading";

        private bool IsVisible 
        {
            set { root.gameObject.SetActive(value); } 
        }

        public void Initialise(IMusicPlayer musicPlayer)
        {
            Helper.AssertIsNotNull(root, loadingText, musicPlayer);
            _musicPlayer = musicPlayer;
        }

        public IEnumerator PerformLongOperation(IEnumerator longOperation, string loadingScreenHint = null)
        {
            IsVisible = true;

            loadingText.text = loadingScreenHint ?? DEFAULT_LOADING_TEXT;
            _musicPlayer.PlayLoadingMusic();

            yield return StartCoroutine(longOperation);

            IsVisible = false;
        }
    }
}
