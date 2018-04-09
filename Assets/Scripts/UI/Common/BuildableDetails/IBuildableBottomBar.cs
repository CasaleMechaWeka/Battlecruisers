using BattleCruisers.Buildables;

namespace BattleCruisers.UI.Common.BuildingDetails
{
    public interface IBuildableBottomBar
    {
        IBuildable Buildable { set; }
        bool IsVisible { get; }
        float Height { get; }
    }
}
