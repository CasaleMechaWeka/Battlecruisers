using UnityEngine;
using UnityEngine.EventSystems;

namespace BattleCruisers.Scenes.Test.Cruisers
{
    public class ClickHandler : MonoBehaviour, IPointerClickHandler
    {
        public void OnPointerClick(PointerEventData eventData)
        {
            Debug.Log("ClickHandler.OnPointerClick()");
        }
    }
}