using BattleCruisers.UI.Commands;

namespace BattleCruisers.Buildables
{
    public interface IRepairable : IDamagable
    {
        float HealthGainPerDroneS { get; }
        IParameterisedCommand<float> RepairCommand { get; }
    }
}
