using System;

namespace BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Cruisers
{
    public class PvPCruiserDestroyedEventArgs : EventArgs
    {
        /// <summary>
        /// True if the AI cruiser was destroyed, false if the player cruiser was destroyed.
        /// </summary>
        public bool WasPlayerVictory { get; }

        public PvPCruiserDestroyedEventArgs(bool wasPlayerVictory)
        {
            WasPlayerVictory = wasPlayerVictory;
        }
    }

    public interface IPvPCruiserDestroyedMonitor
    {
        event EventHandler<PvPCruiserDestroyedEventArgs> CruiserDestroyed;
    }
}
