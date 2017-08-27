using BattleCruisers.UI.Commands;

namespace BattleCruisers.Buildables
{
    public interface IRepairable
    {
        // FELIX  REname
        IParameterisedCommand<float> Repair { get; }
    }
}
