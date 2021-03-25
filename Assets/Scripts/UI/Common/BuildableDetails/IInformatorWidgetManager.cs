using BattleCruisers.Buildables;

namespace BattleCruisers.UI.Common.BuildableDetails
{
    public interface IInformatorWidgetManager
    {
        ITarget SelectedItem { set; }
        IButton ToggleDronesButton { get; }
    }
}
