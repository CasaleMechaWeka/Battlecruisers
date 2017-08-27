namespace BattleCruisers.Buildables
{
    public interface IRepairable : IDamagable
    {
        float HealthGainPerDroneS { get; }
        IRepairCommand RepairCommand { get; }
    }
}
