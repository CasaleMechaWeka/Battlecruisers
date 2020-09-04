using UnityEngine;
using UnityEngine.Assertions;

namespace BattleCruisers.UI
{
    public enum TargetState
    {
        Shown, Hidden
    }

    // FELIX  Use, test
    public class SlidingPanel : Panel
    {
		private Vector2 _slideVelocity;

        private float _smoothTimeinS;
        private bool _haveReachedTarget;
        private Vector2 _targetPosition;
        private TargetState _targetState;
        private TargetState TargetState
        {
            get => _targetState;
            set
            {
                _targetState = value;
                _haveReachedTarget = false;

                if (_targetState == TargetState.Shown)
                {
                    _smoothTimeinS = showSmoothTimeInS;
                    _targetPosition = shownPosition;
                }
                else
                {
                    _smoothTimeinS = hideSmoothTimeInS;
                    _targetPosition = hiddenPosition;
                }
            }
        }
        
        public Vector2 hiddenPosition, shownPosition;
        public float showSmoothTimeInS = 0.2f;
        public float hideSmoothTimeInS = 0.5f;
        public float positionEqualityMarginInPixels = 2;

        void Start()
        {
            if (transform.position.Equals(hiddenPosition))
            {
                TargetState = TargetState.Hidden;
            }
            else if (transform.position.Equals(shownPosition))
            {
                TargetState = TargetState.Shown;
            }
            else
            {
                Assert.IsTrue(false, "Starting position should match shown or hidden position.");
            }
        }

        void Update()
        {
            if (_haveReachedTarget)
            {
                return;
            }

            if (Vector2.Distance(transform.position, _targetPosition) <= positionEqualityMarginInPixels)
            {
                _haveReachedTarget = true;
                return;
            }

            transform.position
                = Vector2.SmoothDamp(
                    transform.position,
                    _targetPosition,
                    ref _slideVelocity,
                    _smoothTimeinS);
        }

        public override void Show()
        {
            TargetState = TargetState.Shown;
        }

        public override void Hide()
        {
            TargetState = TargetState.Hidden;
        }
    }
}