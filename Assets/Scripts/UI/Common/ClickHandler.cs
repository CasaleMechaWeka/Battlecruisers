using System;
using UnityEngine.Assertions;

namespace BattleCruisers.UI.Common
{
    public class ClickHandler : IClickHandler
    {
        private readonly float _doubleClickThresholdInS;
        private float _lastClickTime;

        public ClickHandler(float doubleClickThresholdInS)
        {
            Assert.IsTrue(doubleClickThresholdInS > 0);

            _doubleClickThresholdInS = doubleClickThresholdInS;
            _lastClickTime = 0;
        }

        public event EventHandler SingleClick;
        public event EventHandler DoubleClick;

        public void OnClick(float timeSinceGameStartInS)
        {
            if ((timeSinceGameStartInS - _lastClickTime) <= _doubleClickThresholdInS)
            {
                // Double click
                if (DoubleClick != null)
                {
                    DoubleClick.Invoke(this, EventArgs.Empty);
                }
            }
            else
            {
                // Single click
                if (SingleClick != null)
                {
                    SingleClick.Invoke(this, EventArgs.Empty);
                }
            }

            _lastClickTime = timeSinceGameStartInS;
        }
    }
}