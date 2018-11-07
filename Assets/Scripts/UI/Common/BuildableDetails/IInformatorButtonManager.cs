using BattleCruisers.Buildables;

namespace BattleCruisers.UI.Common.BuildableDetails
{
    public interface IInformatorButtonManager
    {
        IBuildable Buildable { set; }
        IButton ToggleDronesButton { get; }
    }
}
