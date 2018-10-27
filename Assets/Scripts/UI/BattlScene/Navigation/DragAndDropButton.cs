using UnityEngine;
using UnityEngine.EventSystems;

namespace BattleCruisers.UI.BattleScene.Navigation
{
    public class DragAndDropButton : 
        MonoBehaviour, 
        IDragHandler,
        IBeginDragHandler
    {
        private IPositionValidator _positionValidator;
        // Offset adjustment (ie, mouse position relative to our center position),
        // to avoid button "jumping" to mouse position when first clicked :)
        private Vector2 _mouseToCenterOffset;

        // FELIX  Replace with Initialise() :)
        private void Start()
        {
            _mouseToCenterOffset = new Vector2();

            // FELIX  Inject :P
            // FELIX  Don't want validator, want clamper!!  Otherwise won't go perfectly
            // to area edge if mouse is moved quickly :/
            _positionValidator
                = new TrianglePositionValidator(
                    bottomLeftVertex: new Vector2(500, 500),
                    bottomRightVertex: new Vector2(1000, 500),
                    topCenterVertex: new Vector2(750, 1000));
        }

        public void OnBeginDrag(PointerEventData eventData)
        {
            _mouseToCenterOffset = (Vector2)transform.position - eventData.position;
        }

        public void OnDrag(PointerEventData eventData)
        {
            Debug.Log("OnDrag()  eventData.delta: " + eventData.delta);

            Vector2 adjustedTargetPosition = eventData.position + _mouseToCenterOffset;

            if (_positionValidator.IsValid(adjustedTargetPosition))
            {
                transform.position = adjustedTargetPosition;
            }
        }
    }
}