using UnityEngine;
using UnityEngine.Assertions;

namespace BattleCruisers.UI
{
    public class LoadingScreenController : MonoBehaviour, ILoadingScreen
    {
        public bool IsVisible 
        {
            get { return gameObject.activeSelf; }
            set { gameObject.SetActive(value); } 
        }
    }
}
