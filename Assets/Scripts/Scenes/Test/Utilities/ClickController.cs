using System;
using UnityEngine;
using UnityEngine.EventSystems;

namespace BattleCruisers.Scenes.Test.Utilities
{
    public class ClickController : MonoBehaviour, IPointerClickHandler
    {
        public void OnPointerClick(PointerEventData eventData)
        {
            Debug.Log(DateTime.Now + ":  " + name + " OnPointerClick()");
        }
    }
}