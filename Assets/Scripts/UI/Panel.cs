using UnityEngine;

namespace BattleCruisers.UI
{
    public class Panel : MonoBehaviour, IPanel
    {
        public void Show()
        {
            gameObject.SetActive(true);
        }

        public void Hide()
        {
            gameObject.SetActive(false);
        }
    }
}