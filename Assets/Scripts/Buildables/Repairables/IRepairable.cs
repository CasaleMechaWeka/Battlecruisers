using BattleCruisers.Utils.PlatformAbstractions.UI;

namespace BattleCruisers.Buildables.Repairables
{
    public interface IRepairable : IDamagable
    {
        float HealthGainPerDroneS { get; }
        IRepairCommand RepairCommand { get; }

        // FELIX  Remove once have drone aniamtions :)
        ITextMesh NumOfRepairDronesText { get; }
    }
}
