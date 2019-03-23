using System;

namespace BattleCruisers.Cruisers
{
    public class CruiserDestroyedEventArgs : EventArgs
    {
        /// <summary>
        /// True if the AI cruiser was destroyed, false if the player cruiser was destroyed.
        /// </summary>
        public bool WasPlayerVictory { get; }

        public CruiserDestroyedEventArgs(bool wasPlayerVictory)
        {
            WasPlayerVictory = WasPlayerVictory;
        }
    }

    public interface ICruiserDestroyedMonitor
    {
        event EventHandler<CruiserDestroyedEventArgs> CruiserDestroyed;
    }
}