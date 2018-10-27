using BattleCruisers.Utils;
using BattleCruisers.Utils.Clamper;
using UnityEngine;
using UnityEngine.EventSystems;

namespace BattleCruisers.UI.BattleScene.Navigation
{
    public class DragAndDropButton : MonoBehaviour, IDragHandler
    {
        private IPositionClamper _positionClamper;
        private Vector2 _halfSize;

        // FELIX  Replace with Initialise() :)
        private void Start()
        {
            // FELIX  Inject :P
            // FELIX  Don't want validator, want clamper!!  Otherwise won't go perfectly
            // to area edge if mouse is moved quickly :/
            _positionClamper
                = new TrianglePositionClamper(
                    bottomLeftVertex: new Vector2(500, 500),
                    bottomRightVertex: new Vector2(1000, 500),
                    topCenterVertex: new Vector2(750, 1000));

            RectTransform rectTransform = transform.Parse<RectTransform>();
            _halfSize = rectTransform.sizeDelta / 2;
        }

        public void OnDrag(PointerEventData eventData)
        {
            // FELIX
            //Debug.Log("OnDrag()  eventData.delta: " + eventData.delta);

            Vector2 desiredBottomLeftPosition = (Vector2)transform.position + eventData.delta;
            Vector2 desiredCenterPosition = desiredBottomLeftPosition + _halfSize;
            Vector2 clampedCenterPosition = _positionClamper.Clamp(desiredCenterPosition);
            Vector2 clampedBottomLeftPosition = clampedCenterPosition - _halfSize;

            Debug.Log("OnDrag()  clampedPosition: " + clampedCenterPosition + "  halfSize: " + _halfSize + "  bottomLeftPosition: " + clampedBottomLeftPosition);

            transform.position = clampedBottomLeftPosition;
        }
    }
}