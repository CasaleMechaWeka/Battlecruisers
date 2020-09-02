using UnityEngine;

namespace BattleCruisers.UI
{
    public class Panel : MonoBehaviour, IPanel
    {
        public virtual void Show()
        {
            gameObject.SetActive(true);
        }

        public virtual void Hide()
        {
            gameObject.SetActive(false);
        }
    }
}