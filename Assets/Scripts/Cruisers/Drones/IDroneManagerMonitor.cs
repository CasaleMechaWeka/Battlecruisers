using System;

namespace BattleCruisers.Cruisers.Drones
{
    public interface IDroneManagerMonitor
    {
        event EventHandler DroneNumIncreased;
        event EventHandler IdleDronesStarted;
    }
}