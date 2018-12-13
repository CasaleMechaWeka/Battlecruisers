using BattleCruisers.Buildables;

namespace BattleCruisers.UI.Common.BuildableDetails
{
    public interface IBuildableDetails<TItem> : IComparableItemDetails<TItem> where TItem : IBuildable
    {
        IButton DroneFocusButtonNEW { get; }

        void ShowBuildableDetails(TItem buildable);
    }
}
