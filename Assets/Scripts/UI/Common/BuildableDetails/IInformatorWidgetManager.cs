using BattleCruisers.Buildables;

namespace BattleCruisers.UI.Common.BuildableDetails
{
    public interface IInformatorWidgetManager
    {
        IBuildable Buildable { set; }
        IButton ToggleDronesButton { get; }
    }
}
