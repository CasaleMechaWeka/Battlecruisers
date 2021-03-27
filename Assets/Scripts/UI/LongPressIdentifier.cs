using BattleCruisers.Utils;
using BattleCruisers.Utils.BattleScene.Update;
using BattleCruisers.Utils.PlatformAbstractions.Time;
using System;

// FELIX  test
// FELIX  Tidy up namespace?
namespace BattleCruisers.UI
{
    public class LongPressIdentifier : ILongPressIdentifier
    {
        private readonly IPointerUpDownEmitter _button;
        private readonly ITime _time;
        private readonly IUpdater _updater;
        private readonly float _intervalLengthS;
        private float _longPressDurationS;

        public int IntervalNumber { get; private set; }

        public event EventHandler LongPressStart;
        public event EventHandler LongPressEnd;
        public event EventHandler LongPressInterval;

        public LongPressIdentifier(
            IPointerUpDownEmitter button,
            ITime time,
            IUpdater updater,
            float intervalLengthS)
        {
            Helper.AssertIsNotNull(button, time, updater);

            _button = button;
            _time = time;
            _updater = updater;
            _intervalLengthS = intervalLengthS;

            IntervalNumber = 0;

            _button.PointerDown += _button_PointerDown;
            _button.PointerUp += _button_PointerUp;
        }

        private void _button_PointerDown(object sender, EventArgs e)
        {
            _longPressDurationS = 0;
            IntervalNumber = 0;

            _updater.Updated += _updater_Updated;

            LongPressStart?.Invoke(this, EventArgs.Empty);
        }

        private void _button_PointerUp(object sender, EventArgs e)
        {
            _updater.Updated -= _updater_Updated;
            LongPressEnd?.Invoke(this, EventArgs.Empty);
        }

        private void _updater_Updated(object sender, EventArgs e)
        {
            _longPressDurationS += _time.UnscaledDeltaTime;

            int newIntervalNum = (int)(_longPressDurationS / _intervalLengthS);

            if (newIntervalNum != IntervalNumber)
            {
                IntervalNumber = newIntervalNum;
                LongPressInterval?.Invoke(this, EventArgs.Empty);
            }
        }
    }
}