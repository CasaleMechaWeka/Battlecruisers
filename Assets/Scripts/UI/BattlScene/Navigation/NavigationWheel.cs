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
    public class NavigationWheel : ClickableTogglable, INavigationWheel, IDragHandler
    {
        private IPositionClamper _positionClamper;
        private Vector2 _halfSize;
        private IList<GameObject> _activeFeedbacks;
        private FilterToggler _filterToggler;

        protected override bool ShowPressedFeedback => false;

        private Vector2 _centerPosition;
        public Vector2 CenterPosition
        {
            get { return _centerPosition; }
            private set { SetCenterPosition(value, PositionChangeSource.NavigationWheel); }
        }

        private Image _wheel;
        protected override MaskableGraphic Graphic => _wheel;

        public event EventHandler<PositionChangedEventArgs> CenterPositionChanged;

        public void Initialise(
            IPositionClamper positionClamper, 
            GameObject parentActiveFeedback, 
            IBroadcastingFilter shouldBeEnabledFilter)
        {
            base.Initialise();

            Helper.AssertIsNotNull(positionClamper, parentActiveFeedback, shouldBeEnabledFilter);

            _positionClamper = positionClamper;

            RectTransform rectTransform = transform.Parse<RectTransform>();
            Vector2 scaleAdjustedSizeDelta = rectTransform.sizeDelta * rectTransform.lossyScale;
            _halfSize = scaleAdjustedSizeDelta / 2;

            GameObject activeFeedback = transform.FindNamedComponent<Image>("ActiveFeedback").gameObject;
            _activeFeedbacks = new List<GameObject>()
            {
                activeFeedback,
                parentActiveFeedback
            };

            _wheel = GetComponent<Image>();
            Assert.IsNotNull(_wheel);

            SetFeedbackVisibility(isVisible: false);

            this.CenterPosition = Vector2.zero;

            _filterToggler = new FilterToggler(this, shouldBeEnabledFilter);
        }

        public void OnDrag(PointerEventData eventData)
        {
            Vector2 desiredBottomLeftPosition = (Vector2)transform.position + eventData.delta;
            CenterPosition = desiredBottomLeftPosition + _halfSize;
        }

        public override void OnPointerDown(PointerEventData eventData)
        {
            base.OnPointerDown(eventData);
            SetFeedbackVisibility(isVisible: true);
        }

        public override void OnPointerUp(PointerEventData eventData)
        {
            base.OnPointerUp(eventData);
            SetFeedbackVisibility(isVisible: false);
        }

        private void SetFeedbackVisibility(bool isVisible)
        {
            foreach (GameObject feedback in _activeFeedbacks)
            {
                feedback.gameObject.SetActive(isVisible);
            }
        }

        protected override void OnClicked()
        {
            // empty
        }

        public void SetCenterPosition(Vector2 centerPosition)
        {
            SetCenterPosition(centerPosition, PositionChangeSource.Other);
        }

        private void SetCenterPosition(Vector2 centerPosition, PositionChangeSource source)
        {
            Vector2 desiredCenterPosition = centerPosition;
            Vector2 clampedCenterPosition = _positionClamper.Clamp(desiredCenterPosition);
            _centerPosition = clampedCenterPosition;
            Vector2 clampedBottomLeftPosition = clampedCenterPosition - _halfSize;

            transform.position = clampedBottomLeftPosition;

            Logging.Log(Tags.NAVIGATION_WHEEL, $"desiredCenterPosition: {desiredCenterPosition}  clampedCenterPosition: {clampedCenterPosition}  clampedBottomLeftPosition: {clampedBottomLeftPosition}");

            CenterPositionChanged?.Invoke(this, new PositionChangedEventArgs(source));
        }
    }
}