using BattleCruisers.UI.Music;
using BattleCruisers.Utils;
using System.Collections;
using UnityEngine;

namespace BattleCruisers.UI
{
    public class LoadingScreenController : MonoBehaviour, ILoadingScreen
    {
        private IMusicPlayer _musicPlayer;

        public Canvas root; 

        private bool IsVisible 
        {
            set { root.gameObject.SetActive(value); } 
        }

        public void Initialise(IMusicPlayer musicPlayer)
        {
            Helper.AssertIsNotNull(root, musicPlayer);
            _musicPlayer = musicPlayer;
        }

        public IEnumerator PerformLongOperation(IEnumerator longOperation)
        {
            IsVisible = true;

            _musicPlayer.Stop();

            yield return StartCoroutine(longOperation);

            IsVisible = false;
        }
    }
}
