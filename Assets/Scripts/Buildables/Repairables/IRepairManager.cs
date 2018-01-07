using System;
using BattleCruisers.Cruisers.Drones;

namespace BattleCruisers.Buildables.Repairables
{
    public interface IRepairManager : IDisposable
    {
        void Repair(float deltaTimeInS);
        IDroneConsumer GetDroneConsumer(IRepairable repairable);
    }
}
