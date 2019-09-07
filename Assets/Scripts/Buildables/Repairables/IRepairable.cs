using UnityEngine;

namespace BattleCruisers.Buildables.Repairables
{
    public interface IRepairable : IDamagable
    {
        float HealthGainPerDroneS { get; }
        IRepairCommand RepairCommand { get; }
        Vector2 Size { get; }
        Vector2 Position { get; set; }
    }
}
