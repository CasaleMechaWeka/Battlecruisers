using System;

namespace BattleCruisers.Utils.Timers
{
    public class CountdownEventArgs : EventArgs
    {
        public int SecondsRemaining { get; }

        public CountdownEventArgs(int secondsReamining)
        {
            SecondsRemaining = secondsReamining;
        }
    }

    public interface ICountdown
    {
        event EventHandler<CountdownEventArgs> OnSecondPassed;

        void Start(int durationInS, Action onCompletion);
        void Cancel();
        void OnUpdate(float timeInS);
    }
}
