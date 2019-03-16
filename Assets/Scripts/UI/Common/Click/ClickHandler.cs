using System;
using UnityEngine.Assertions;

namespace BattleCruisers.UI.Common.Click
{
    public class ClickHandler : IClickHandler
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

        public void OnClick(float timeSinceGameStartInS)
        {
            if ((timeSinceGameStartInS - _lastClickTime) <= _doubleClickThresholdInS)
            {
                // Double click
                DoubleClick?.Invoke(this, EventArgs.Empty);

                // Don't want third click within threshold to count as a second double click :)
                _lastClickTime = float.MinValue;
            }
            else
            {
                // Single click
                SingleClick?.Invoke(this, EventArgs.Empty);

                _lastClickTime = timeSinceGameStartInS;
            }
        }
    }
}