using System.Collections;
using UnityEngine;
using UnityEngine.Assertions;

namespace BattleCruisers.UI
{
    public class LoadingScreenController : MonoBehaviour, ILoadingScreen
    {
        public Canvas root; 

        private bool IsVisible 
        {
            set { root.gameObject.SetActive(value); } 
        }

        public void Initialise()
        {
            Assert.IsNotNull(root);
        }

        public IEnumerator PerformLongOperation(IEnumerator longOperation)
        {
            IsVisible = true;

            yield return StartCoroutine(longOperation);

            IsVisible = false;
        }
    }
}
