using BattleCruisers.Utils.PlatformAbstractions.Time;
using BattleCruisers.Utils.Properties;
using UnityEngine;
using UnityEngine.Assertions;

namespace BattleCruisers.UI.Panels
{
    public enum PanelState
    {
        Shown, Hidden, Sliding
    }

    public class SlidingPanel : Panel, ISlidingPanel
    {
		private Vector2 _slideVelocity;
        private bool _isInitialised = false;
        private Vector2 _hiddenPosition, _shownPosition;

        private float _smoothTimeinS;
        private Vector2 _targetPosition;
        private PanelState _targetState;
        private PanelState TargetState
        {
            get => _targetState;
            set
            {
                Assert.IsTrue(value != PanelState.Sliding);

                _targetState = value;
                _state.Value = PanelState.Sliding;

                if (_targetState == PanelState.Shown)
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

        private ISettableBroadcastingProperty<PanelState> _state;
        public IBroadcastingProperty<PanelState> State { get; private set; }

        public float shownPositionYDelta = 500;
        public float showSmoothTimeInS = 0.05f;
        public float hideSmoothTimeInS = 0.2f;
        public float positionEqualityMarginInPixels = 2;

        public void Initialise()
        {
            _hiddenPosition = transform.position;
            float yDelta = transform.lossyScale.y * shownPositionYDelta;
            _shownPosition = new Vector2(transform.position.x, transform.position.y + yDelta);

            _state = new SettableBroadcastingProperty<PanelState>(initialValue: PanelState.Sliding);
            State = new BroadcastingProperty<PanelState>(_state);
            TargetState = PanelState.Hidden;

            _isInitialised = true;
        }

        void Update()
        {
            if (!_isInitialised
                || _state.Value != PanelState.Sliding)
            {
                return;
            }

            if (Vector2.Distance(transform.position, _targetPosition) <= positionEqualityMarginInPixels)
            {
                _state.Value = TargetState;
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
            TargetState = PanelState.Shown;
        }

        public override void Hide()
        {
            TargetState = PanelState.Hidden;
        }
    }
}