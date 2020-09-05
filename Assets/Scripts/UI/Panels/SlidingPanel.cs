using UnityCommon.PlatformAbstractions.Time;
using UnityEngine;

namespace BattleCruisers.UI.Panels
{
    public enum TargetState
    {
        Shown, Hidden
    }

    public class SlidingPanel : Panel
    {
		private Vector2 _slideVelocity;
        private bool _isInitialised = false;
        private Vector2 _hiddenPosition, _shownPosition;

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
                    _targetPosition = _shownPosition;
                }
                else
                {
                    _smoothTimeinS = hideSmoothTimeInS;
                    _targetPosition = _hiddenPosition;
                }
            }
        }
        
        public float shownPositionYDelta = 500;
        public float showSmoothTimeInS = 0.05f;
        public float hideSmoothTimeInS = 0.2f;
        public float positionEqualityMarginInPixels = 2;

        public void Initialise()
        {
            _hiddenPosition = transform.position;
            float yDelta = transform.lossyScale.y * shownPositionYDelta;
            _shownPosition = new Vector2(transform.position.x, transform.position.y + yDelta);
            TargetState = TargetState.Hidden;
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
                    _smoothTimeinS,
                    maxSpeed: float.MaxValue,
                    deltaTime: TimeBC.Instance.UnscaledDeltaTime);
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