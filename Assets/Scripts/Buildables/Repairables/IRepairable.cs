using BattleCruisers.Utils.UIWrappers;

namespace BattleCruisers.Buildables.Repairables
{
    public interface IRepairable : IDamagable
    {
        float HealthGainPerDroneS { get; }
        IRepairCommand RepairCommand { get; }
        ITextMesh NumOfRepairDronesText { get; }
    }
}
