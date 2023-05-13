using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Utils;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Utils.BattleScene.Update;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Utils.PlatformAbstractions.Time;
using System;

namespace BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.UI
{
    public class PvPLongPressIdentifier : IPvPLongPressIdentifier
    {
        private readonly IPvPPointerUpDownEmitter _button;
        private readonly IPvPTime _time;
        private readonly IPvPUpdater _updater;
        private readonly float _intervalLengthS;
        private float _longPressDurationS;

        public int IntervalNumber { get; private set; }

        public event EventHandler LongPressStart;
        public event EventHandler LongPressEnd;
        public event EventHandler LongPressInterval;

        public PvPLongPressIdentifier(
            IPvPPointerUpDownEmitter button,
            IPvPTime time,
            IPvPUpdater updater,
            float intervalLengthS)
        {
            PvPHelper.AssertIsNotNull(button, time, updater);

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