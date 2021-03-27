using BattleCruisers.Buildables;

namespace BattleCruisers.UI.Common.BuildableDetails
{
    public interface IInformatorButtons
    {
        ITarget SelectedItem { set; }
        IButton ToggleDronesButton { get; }
    }
}
