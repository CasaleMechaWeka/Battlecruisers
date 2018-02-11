using BattleCruisers.Cruisers;

namespace BattleCruisers.UI.Common.BuildingDetails
{
    public interface ICruiserDetails : IComparableItemDetails<ICruiser>
    {
        void ShowCruiserDetails(ICruiser cruiser);
    }
}
