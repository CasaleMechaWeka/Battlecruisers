using BattleCruisers.Buildables;

namespace BattleCruisers.UI.Common.BuildableDetails
{
    public interface IButtonManager
    {
        IBuildable Buildable { set; }
        IButton ToggleDronesButton { get; }
    }
}
