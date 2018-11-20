using BattleCruisers.UI.BattleScene.Buttons;
using BattleCruisers.UI.Filters;
using BattleCruisers.Utils;
using BattleCruisers.Utils.Clamping;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;
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

        private Vector2 _centerPosition;
        public Vector2 CenterPosition
        {
            get { return _centerPosition; }
            set
            {
                Vector2 desiredCenterPosition = value;
                Vector2 clampedCenterPosition = _positionClamper.Clamp(desiredCenterPosition);
                _centerPosition = clampedCenterPosition;
                Vector2 clampedBottomLeftPosition = clampedCenterPosition - _halfSize;

                transform.position = clampedBottomLeftPosition;

                if (CenterPositionChanged != null)
                {
                    CenterPositionChanged.Invoke(this, EventArgs.Empty);
                }
            }
        }

        public event EventHandler CenterPositionChanged;

        public void Initialise(
            IPositionClamper positionClamper, 
            GameObject parentActiveFeedback, 
            IBroadcastingFilter shouldBeEnabledFilter)
        {
            Helper.AssertIsNotNull(positionClamper, parentActiveFeedback, shouldBeEnabledFilter);

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

            _centerPosition = (Vector2)transform.position + _halfSize;

            ButtonWrapper buttonWrapper = GetComponent<ButtonWrapper>();
            Assert.IsNotNull(buttonWrapper);
            buttonWrapper.Initialise(shouldBeEnabledFilter);

            // FELIX  TEMP  This does everything :/  1. Disable element  2. Makes slightly transparent.
            // => Create ITogglable.Enabled & pass to TogglableElement?
            this.enabled = false;
        }

        public void OnDrag(PointerEventData eventData)
        {
            Vector2 desiredBottomLeftPosition = (Vector2)transform.position + eventData.delta;
            CenterPosition = desiredBottomLeftPosition + _halfSize;
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