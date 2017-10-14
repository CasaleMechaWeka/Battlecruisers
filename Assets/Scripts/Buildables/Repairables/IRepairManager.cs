using System;
using BattleCruisers.Drones;

namespace BattleCruisers.Buildables.Repairables
{
    public interface IRepairManager : IDisposable
    {
        void Repair(float deltaTimeInS);
        IDroneConsumer GetDroneConsumer(IRepairable repairable);
    }
}
