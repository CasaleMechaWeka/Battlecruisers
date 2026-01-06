using System;
using UnityEngine.Assertions;

namespace BattleCruisers.UI.Common.Click
{
    public class ClickHandler
    {
        private readonly float _doubleClickThresholdInS;
        private float _lastClickTime;

        public ClickHandler(float doubleClickThresholdInS)
        {
            Assert.IsTrue(doubleClickThresholdInS > 0);

            _doubleClickThresholdInS = doubleClickThresholdInS;
            _lastClickTime = float.MinValue;
        }

        public event EventHandler SingleClick;
        public event EventHandler DoubleClick;
        public event EventHandler TripleClick;

        private int _clickCount = 0;
        private float _firstClickTime;

        public void OnClick(float timeSinceGameStartInS)
        {
            _clickCount++;

            if (_clickCount == 1)
            {
                // First click
                _firstClickTime = timeSinceGameStartInS;
                _lastClickTime = timeSinceGameStartInS;
                return;
            }

            if ((timeSinceGameStartInS - _lastClickTime) <= _doubleClickThresholdInS)
            {
                // Click within threshold
                _lastClickTime = timeSinceGameStartInS;

                if (_clickCount == 2)
                {
                    // Double click - wait to see if triple click comes
                    return;
                }
                else if (_clickCount == 3)
                {
                    // Triple click
                    TripleClick?.Invoke(this, EventArgs.Empty);
                    _clickCount = 0;
                    _lastClickTime = float.MinValue;
                }
            }
            else
            {
                // Click outside threshold - process previous clicks
                if (_clickCount >= 3)
                {
                    // Should not happen, but handle gracefully
                    TripleClick?.Invoke(this, EventArgs.Empty);
                }
                else if (_clickCount == 2)
                {
                    DoubleClick?.Invoke(this, EventArgs.Empty);
                }
                else
                {
                    SingleClick?.Invoke(this, EventArgs.Empty);
                }

                // Reset for new sequence
                _clickCount = 1;
                _firstClickTime = timeSinceGameStartInS;
                _lastClickTime = timeSinceGameStartInS;
            }
        }
    }
}