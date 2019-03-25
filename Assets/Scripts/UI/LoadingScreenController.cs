using System.Collections;
using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.UI;

namespace BattleCruisers.UI
{
    public class LoadingScreenController : MonoBehaviour, ILoadingScreen
    {
        public Canvas root;
        public Text loadingText;

        private const string DEFAULT_LOADING_TEXT = "Loading";

        private bool IsVisible 
        {
            set { root.gameObject.SetActive(value); } 
        }

        public void Initialise()
        {
            Assert.IsNotNull(root);
            Assert.IsNotNull(loadingText);
        }

        public IEnumerator PerformLongOperation(IEnumerator longOperation, string loadingScreenHint = null)
        {
            IsVisible = true;

            loadingText.text = loadingScreenHint ?? DEFAULT_LOADING_TEXT;
            yield return StartCoroutine(longOperation);

            IsVisible = false;
        }
    }
}
