using BattleCruisers.Utils;
using BattleCruisers.Utils.Clamper;
using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.EventSystems;

namespace BattleCruisers.UI.BattleScene.Navigation
{
    public class NavigationWheel : MonoBehaviour, IDragHandler
    {
        private IPositionClamper _positionClamper;
        private Vector2 _halfSize;

        public void Initialise(IPositionClamper positionClamper)
        {
            Assert.IsNotNull(positionClamper);
            _positionClamper = positionClamper;

            RectTransform rectTransform = transform.Parse<RectTransform>();
            _halfSize = rectTransform.sizeDelta / 2;
        }

        public void OnDrag(PointerEventData eventData)
        {
            Vector2 desiredBottomLeftPosition = (Vector2)transform.position + eventData.delta;
            Vector2 desiredCenterPosition = desiredBottomLeftPosition + _halfSize;
            Vector2 clampedCenterPosition = _positionClamper.Clamp(desiredCenterPosition);
            Vector2 clampedBottomLeftPosition = clampedCenterPosition - _halfSize;

            // FELIX  Remove :)
            Debug.Log("OnDrag()  clampedPosition: " + clampedCenterPosition + "  halfSize: " + _halfSize + "  bottomLeftPosition: " + clampedBottomLeftPosition);

            transform.position = clampedBottomLeftPosition;
        }
    }
}