using System;
using BattleCruisers.Drones;

namespace BattleCruisers.Buildables.Repairables
{
    public interface IRepairManager : IDisposable
    {
        void Repair(float deltaTimeInS);

        /// <returns>
        /// The drone consumer for the given repairable if it is currently
        /// repairable (not on full health), or null if the given repairable
        /// is not currently repairable (on full health).
        /// </returns>
        IDroneConsumer GetDroneConsumer(IRepairable repairable);
    }
}
