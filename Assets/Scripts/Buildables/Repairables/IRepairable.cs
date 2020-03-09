using UnityEngine;

namespace BattleCruisers.Buildables.Repairables
{
    public interface IRepairable : IDamagable
    {
        float HealthGainPerDroneS { get; }
        IRepairCommand RepairCommand { get; }

        /// <summary>
        /// Not the whole repairable size might be a good location for drones.
        /// Eg, for cruisers the top third is empty air (besides the mast).
        /// </summary>
        Vector2 DroneAreaSize { get; }
        Vector2 DroneAreaPosition { get; }
    }
}
