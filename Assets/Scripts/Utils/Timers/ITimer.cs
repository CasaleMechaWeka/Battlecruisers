using System;

namespace BattleCruisers.Utils.Timers
{
    public class TimerEventArgs : EventArgs
    {
        public int SecondsElapsed { get; private set; }

        public TimerEventArgs(int secondsElapsed)
        {
            SecondsElapsed = secondsElapsed;
        }
    }

    public interface ITimer
    {
        event EventHandler<TimerEventArgs> OnSecondPassed;

        void Start();
        void Stop();
        void OnUpdate(float timeInS);
    }
}
