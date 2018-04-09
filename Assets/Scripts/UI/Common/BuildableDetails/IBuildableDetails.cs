using BattleCruisers.Buildables;

namespace BattleCruisers.UI.Common.BuildableDetails
{
    public interface IBuildableDetails<TItem> : IComparableItemDetails<TItem> where TItem : IBuildable
    {
        void ShowBuildableDetails(TItem buildable, bool allowDelete);
    }
}
