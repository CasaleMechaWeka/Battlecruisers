using UnityEngine;
using UnityEngine.EventSystems;

namespace BattleCruisers.Scenes.Test.UI
{
    public class TargetButton : MonoBehaviour, IPointerClickHandler
    {
        public void Initialise()
        {

        }

        public void OnPointerClick(PointerEventData eventData)
        {
            Debug.Log("yo :D");
        }
    }
}