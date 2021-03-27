using BattleCruisers.Utils;
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
		private Vector2 _slidePositionVelocity, _sliderScaleVelocity;
        private bool _isInitialised = false;
        private Vector2 _hiddenPosition, _shownPosition, _hiddenScale;
        private bool _positionDone, _scaleDone;

        private float _smoothTimeinS;
        private Vector2 _targetPosition, _targetScale;
        private PanelState _targetState;
        public PanelState TargetState
        {
            get => _targetState;
            private set
            {
                Assert.IsTrue(value != PanelState.Sliding);
                Logging.Log(Tags.SLIDING_PANEL, $"Target state: {_targetState} > {value}");

                _targetState = value;
                _state.Value = PanelState.Sliding;
                _positionDone = false;
                _scaleDone = false;

                if (_targetState == PanelState.Shown)
                {
                    _smoothTimeinS = showSmoothTimeInS;
                    _targetPosition = _shownPosition;
                    _targetScale = shownScale;
                }
                else
                {
                    _smoothTimeinS = hideSmoothTimeInS;
                    _targetPosition = _hiddenPosition;
                    _targetScale = _hiddenScale;
                }
            }
        }

        private ISettableBroadcastingProperty<PanelState> _state;
        public IBroadcastingProperty<PanelState> State { get; private set; }

        public float shownPositionYDelta = 500;
        public float showSmoothTimeInS = 0.05f;
        public float hideSmoothTimeInS = 0.2f;
        public float positionEqualityMarginInPixels = 2;

        public bool ignoreLocalScale = false;
        public bool changeScale = false;
        public Vector2 shownScale = new Vector2(1, 1);
        public float scaleEqualityMargin = 0.005f;

        public void Initialise()
        {
            if (ignoreLocalScale)
            {
                Assert.IsFalse(changeScale);
            }

            _hiddenPosition = transform.localPosition;
            float yDelta = shownPositionYDelta;
            if (!ignoreLocalScale)
            {
                yDelta *= transform.lossyScale.y;
            }
            _shownPosition = new Vector2(transform.localPosition.x, transform.localPosition.y + yDelta);

            _hiddenScale = transform.localScale;

            _state = new SettableBroadcastingProperty<PanelState>(initialValue: PanelState.Sliding);
            State = new BroadcastingProperty<PanelState>(_state);
            TargetState = PanelState.Hidden;

            _isInitialised = true;

            Logging.Log(Tags.SLIDING_PANEL, $"{this}  Hidden position: {_hiddenPosition}  Scale: {_hiddenScale}  Shown position: {_shownPosition}  Scale: {shownScale}");
        }

        void Update()
        {
            if (!_isInitialised
                || _state.Value != PanelState.Sliding)
            {
                return;
            }

            if (_positionDone && _scaleDone)
            {
                _state.Value = TargetState;
                return;
            }

            _positionDone = AdjustPosition();
            _scaleDone = AdjustScale();
        }

        private bool AdjustPosition()
        {
            if (_positionDone
                || Vector2.Distance(transform.localPosition, _targetPosition) <= positionEqualityMarginInPixels)
            {
                return true;
            }

            transform.localPosition
                = Vector2.SmoothDamp(
                    transform.localPosition,
                    _targetPosition,
                    ref _slidePositionVelocity,
                    _smoothTimeinS,
                    maxSpeed: float.MaxValue,
                    deltaTime: TimeBC.Instance.UnscaledDeltaTime);
            return false;
        }

        private bool AdjustScale()
        { 
            if (!changeScale
                || _scaleDone
                || Vector2.Distance(transform.localScale, _targetScale) <= scaleEqualityMargin)
            {
                return true;
            }

            transform.localScale
                = Vector2.SmoothDamp(
                    transform.localScale,
                    _targetScale,
                    ref _sliderScaleVelocity,
                    _smoothTimeinS,
                    maxSpeed: float.MaxValue,
                    deltaTime: TimeBC.Instance.UnscaledDeltaTime);
            return false;
        }

        public override void Show()
        {
            Logging.LogMethod(Tags.SLIDING_PANEL);
            TargetState = PanelState.Shown;
        }

        public override void Hide()
        {
            Logging.LogMethod(Tags.SLIDING_PANEL);
            TargetState = PanelState.Hidden;
        }
    }
}