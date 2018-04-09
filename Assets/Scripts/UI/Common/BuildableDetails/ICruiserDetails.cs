using BattleCruisers.Cruisers;

namespace BattleCruisers.UI.Common.BuildableDetails
{
    public interface ICruiserDetails : IComparableItemDetails<ICruiser>
    {
        void ShowCruiserDetails(ICruiser cruiser);
    }
}
