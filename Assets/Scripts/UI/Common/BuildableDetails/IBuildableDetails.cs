using BattleCruisers.Buildables;

namespace BattleCruisers.UI.Common.BuildingDetails
{
    public interface IBuildableDetails : IComparableItemDetails<IBuildable>
    {
        void ShowBuildableDetails(IBuildable buildable, bool allowDelete);
    }
}
