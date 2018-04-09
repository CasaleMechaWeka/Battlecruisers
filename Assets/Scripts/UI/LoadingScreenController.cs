using System.Collections;
using BattleCruisers.Utils;
using UnityEngine;
using UnityEngine.Assertions;

namespace BattleCruisers.UI
{
    public class LoadingScreenController : MonoBehaviour, ILoadingScreen
    {
        // FELIX  Retrieve programmatically
        public Canvas root; 

        private bool IsVisible 
        {
            // FELIX
            set 
            {
                Logging.Log("LoadingScreenController  set_IsVisible: " + value);
                root.gameObject.SetActive(value); 
            } 
            //set { root.gameObject.SetActive(value); } 
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
