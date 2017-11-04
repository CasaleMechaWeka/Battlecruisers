using BattleCruisers.Cruisers;

namespace BattleCruisers.UI.Common.BuildingDetails
{
    public interface IInBattleCruiserDetails : IComparableItemDetails<ICruiser>
    {
        void ShowCruiserDetails(ICruiser cruiser);
    }
}
