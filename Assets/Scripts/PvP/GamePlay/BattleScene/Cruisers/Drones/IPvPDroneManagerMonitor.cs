using System;

namespace BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Cruisers.Drones
{
    public interface IPvPDroneManagerMonitor
    {
        event EventHandler DroneNumIncreased;
        event EventHandler IdleDronesStarted;
        event EventHandler IdleDronesEnded;
    }
}