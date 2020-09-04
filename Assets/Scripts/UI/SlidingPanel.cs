using UnityEngine;
using UnityEngine.Assertions;

namespace BattleCruisers.UI
{
    public enum TargetState
    {
        Shown, Hidden
    }

    public class SlidingPanel : Panel
    {
		private Vector2 _slideVelocity;
        private bool _isInitialised = false;

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
        public float showSmoothTimeInS = 0.05f;
        public float hideSmoothTimeInS = 0.2f;
        public float positionEqualityMarginInPixels = 2;

        public void Initialise(TargetState startingState)
        {
            TargetState = startingState;
            _isInitialised = true;
        }

        void Update()
        {
            if (_haveReachedTarget
                || !_isInitialised)
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