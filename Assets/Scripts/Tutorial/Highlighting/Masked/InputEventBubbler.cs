using BattleCruisers.Utils;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

namespace BattleCruisers.Tutorial.Highlighting.Masked
{
    // https://answers.unity.com/questions/884262/catch-pointer-events-by-multiple-gameobjects.html?childToView=1727303#answer-1727303
    public class InputEventBubbler : MonoBehaviour, IPointerDownHandler, IDragHandler, IBeginDragHandler, IPointerUpHandler, IEndDragHandler
    {
        private GameObject _newTarget;

        public void OnPointerDown(PointerEventData eventData)
        {
            List<RaycastResult> raycastResults = new List<RaycastResult>();
            EventSystem.current.RaycastAll(eventData, raycastResults);
            _newTarget = raycastResults[1].gameObject; //Array item 1 should be the one next underneath, handy to implement for-loop with check here if necessary.
            Logging.Log(Tags.MASKS, $"Bubbling to {_newTarget}"); // Just make sure you caught the right object

            ExecuteEvents.Execute(_newTarget, eventData, ExecuteEvents.pointerDownHandler);
        }

        public void OnPointerUp(PointerEventData eventData)
        {
            Logging.Log(Tags.MASKS, $"Bubbling to {_newTarget}");
            ExecuteEvents.Execute(_newTarget, eventData, ExecuteEvents.pointerUpHandler);
        }

        public void OnBeginDrag(PointerEventData eventData)
        {
            Logging.Log(Tags.MASKS, $"Bubbling to {_newTarget}");
            ExecuteEvents.Execute(_newTarget, eventData, ExecuteEvents.beginDragHandler);
        }

        public void OnDrag(PointerEventData eventData)
        {
            Logging.Log(Tags.MASKS, $"Bubbling to {_newTarget}");
            ExecuteEvents.Execute(_newTarget, eventData, ExecuteEvents.dragHandler);
        }

        public void OnEndDrag(PointerEventData eventData)
        {
            Logging.Log(Tags.MASKS, $"Bubbling to {_newTarget}");
            ExecuteEvents.Execute(_newTarget, eventData, ExecuteEvents.endDragHandler);
        }
    }
}