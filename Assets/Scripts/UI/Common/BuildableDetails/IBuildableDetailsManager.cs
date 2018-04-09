using BattleCruisers.Buildables.Buildings;
using BattleCruisers.Buildables.Units;
using BattleCruisers.Cruisers;

namespace BattleCruisers.UI.Common.BuildableDetails
{
    public interface IBuildableDetailsManager
    {
        void ShowDetails(IBuilding building, bool allowDelete);
        void ShowDetails(IUnit unit);
        void ShowDetails(ICruiser cruiser);
        void HideDetails();
    }
}
