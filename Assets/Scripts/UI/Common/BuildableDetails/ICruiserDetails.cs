using BattleCruisers.Cruisers;

namespace BattleCruisers.UI.Common.BuildableDetails
{
    // FELIX  Remove :P
    public interface ICruiserDetails : IComparableItemDetails<ICruiser>
    {
        void ShowCruiserDetails(ICruiser cruiser);
    }
}
