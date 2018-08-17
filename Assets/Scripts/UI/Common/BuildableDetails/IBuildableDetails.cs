using BattleCruisers.Buildables;

namespace BattleCruisers.UI.Common.BuildableDetails
{
    public interface IBuildableDetails<TItem> : IComparableItemDetails<TItem> where TItem : IBuildable
    {
        IButton DroneFocusButton { get; }

        void ShowBuildableDetails(TItem buildable);
    }
}
