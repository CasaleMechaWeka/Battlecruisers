using UnityEngine;
using System.Collections;
using UnityEngine.EventSystems;

namespace BattleCruisers.Scenes.Test.Cruisers
{
    public class LittleCruiserController : MonoBehaviour, IPointerClickHandler
    {
        public void OnPointerClick(PointerEventData eventData)
        {
            Debug.Log($"Clicked {eventData.position}");
        }
    }
}