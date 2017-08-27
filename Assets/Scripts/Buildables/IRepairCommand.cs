using BattleCruisers.UI.Commands;

namespace BattleCruisers.Buildables
{
    public interface IRepairCommand : IParameterisedCommand<float>
    {
        IRepairable Repairable { get; }
    }
}
