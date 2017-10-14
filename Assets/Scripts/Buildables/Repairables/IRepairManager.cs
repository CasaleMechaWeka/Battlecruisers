using System;
using BattleCruisers.Drones;

namespace BattleCruisers.Buildables.Repairables
{
    public interface IRepairManager : IDisposable
    {
        void Repair(float deltaTimeInS);

        /// <returns>
        /// The drone consumer for the given repairable, or null if there is no
        /// drone consumer for that repairable.
        /// </returns>
        IDroneConsumer GetDroneConsumer(IRepairable repairable);
    }
}
