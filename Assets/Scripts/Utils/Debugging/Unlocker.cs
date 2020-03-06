using UnityEngine;
using UnityEngine.EventSystems;

namespace BattleCruisers.Utils.Debugging
{
    public class Unlocker : MonoBehaviour, IPointerClickHandler
    {
        private int _numOfClicks = 0;

        public int numOfClicksToUnlock = 7;

        void Start()
        {
            Debug.Log("yo");
        }

        public void OnPointerClick(PointerEventData eventData)
        {
            _numOfClicks++;
            Debug.Log($"{_numOfClicks}/{numOfClicksToUnlock}");

            if (_numOfClicks == numOfClicksToUnlock)
            {
                UnlockEverything();
                enabled = false;
            }
        }

        private void UnlockEverything()
        {

        }
    }
}