using BattleCruisers.Utils;
using BattleCruisers.Utils.Clamping;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace BattleCruisers.UI.BattleScene.Navigation
{
    public class NavigationWheel : MonoBehaviour, 
        INavigationWheel, 
        IDragHandler, 
        IPointerDownHandler,
        IPointerUpHandler
    {
        private IPositionClamper _positionClamper;
        private Vector2 _halfSize;
        private IList<GameObject> _activeFeedbacks;

        public Vector2 CenterPosition { get { return (Vector2)transform.position + _halfSize; } }

        public event EventHandler CenterPositionChanged;

        public void Initialise(IPositionClamper positionClamper, GameObject parentActiveFeedback)
        {
            Helper.AssertIsNotNull(positionClamper, parentActiveFeedback);
            _positionClamper = positionClamper;

            RectTransform rectTransform = transform.Parse<RectTransform>();
            _halfSize = rectTransform.sizeDelta / 2;

            GameObject activeFeedback = transform.FindNamedComponent<Image>("ActiveFeedback").gameObject;
            _activeFeedbacks = new List<GameObject>()
            {
                activeFeedback,
                parentActiveFeedback
            };

            SetFeedbackVisibility(isVisible: false);
        }

        public void OnDrag(PointerEventData eventData)
        {
            Vector2 desiredBottomLeftPosition = (Vector2)transform.position + eventData.delta;
            Vector2 desiredCenterPosition = desiredBottomLeftPosition + _halfSize;
            Vector2 clampedCenterPosition = _positionClamper.Clamp(desiredCenterPosition);
            Vector2 clampedBottomLeftPosition = clampedCenterPosition - _halfSize;

            transform.position = clampedBottomLeftPosition;

            if (CenterPositionChanged != null)
            {
                CenterPositionChanged.Invoke(this, EventArgs.Empty);
            }
        }

        public void OnPointerDown(PointerEventData eventData)
        {
            SetFeedbackVisibility(isVisible: true);
        }

        public void OnPointerUp(PointerEventData eventData)
        {
            SetFeedbackVisibility(isVisible: false);
        }

        private void SetFeedbackVisibility(bool isVisible)
        {
            foreach (GameObject feedback in _activeFeedbacks)
            {
                feedback.gameObject.SetActive(isVisible);
            }
        }
    }
}