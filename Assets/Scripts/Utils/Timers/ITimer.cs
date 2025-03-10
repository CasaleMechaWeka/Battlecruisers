using System;

namespace BattleCruisers.Utils.Timers
{
    public class TimerEventArgs : EventArgs
    {
        public int SecondsElapsed { get; }

        public TimerEventArgs(int secondsElapsed)
        {
            SecondsElapsed = secondsElapsed;
        }
    }

    public interface ITimer
    {
        bool IsRunning { get; }

        event EventHandler<TimerEventArgs> OnSecondPassed;

        void Start();
        void Stop();
        void OnUpdate(float timeInS);
    }
}
