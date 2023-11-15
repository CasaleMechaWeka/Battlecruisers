using System;

namespace BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Cruisers.Drones
{
    public enum PvPDroneConsumerState
    {
        Active,     // Has the exact number of drones required
        Focused,    // Has more than the number of drones required
        Idle,       // Has no drones
        AllFocused  // Has every single drone available
    }

    public class PvPDroneNumChangedEventArgs : EventArgs
    {
        public int NewNumOfDrones { get; }

        public PvPDroneNumChangedEventArgs(int newNumOfDrones)
        {
            NewNumOfDrones = newNumOfDrones;
        }
    }

    public class PvPDroneStateChangedEventArgs : EventArgs
    {
        public PvPDroneConsumerState OldState { get; }
        public PvPDroneConsumerState NewState { get; }

        public PvPDroneStateChangedEventArgs(PvPDroneConsumerState oldState, PvPDroneConsumerState newState)
        {
            OldState = oldState;
            NewState = newState;
        }
    }

    /// <summary>
    /// Used by buildings, units and cruisers.  Anything that needs to be built or repaired.
    /// </summary>
    public interface IPvPDroneConsumer
    {
        int NumOfDrones { get; set; }
        int NumOfDronesRequired { get; }
        int NumOfSpareDrones { get; }
        PvPDroneConsumerState State { get; }

        event EventHandler<PvPDroneNumChangedEventArgs> DroneNumChanged;
        event EventHandler<PvPDroneStateChangedEventArgs> DroneStateChanged;
    }
}
