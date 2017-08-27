using BattleCruisers.UI.Commands;

namespace BattleCruisers.Buildables
{
    public interface IRepairable
    {
        IParameterisedCommand<float> Repair { get; }
    }
}
