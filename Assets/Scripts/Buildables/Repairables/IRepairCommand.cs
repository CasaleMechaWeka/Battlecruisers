using BattleCruisers.UI.Commands;

namespace BattleCruisers.Buildables.Repairables
{
    public interface IRepairCommand : IParameterisedCommand<float>
    {
        IRepairable Repairable { get; }
    }
}
