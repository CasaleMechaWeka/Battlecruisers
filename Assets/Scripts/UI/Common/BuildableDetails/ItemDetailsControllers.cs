using BattleCruisers.Buildables.Buildings;
using BattleCruisers.Buildables.Units;
using BattleCruisers.Cruisers;
using BattleCruisers.Utils;

namespace BattleCruisers.UI.Common.BuildingDetails
{
    public class ItemDetailsControllers : IItemDetailsControllers
    {
        public IComparableItemDetails<IBuilding> BuildingDetails { get; private set; }
        public IComparableItemDetails<IUnit> UnitDetails { get; private set; }
        public IComparableItemDetails<ICruiser> HullDetails { get; private set; }

        public ItemDetailsControllers(
            IComparableItemDetails<IBuilding> buildingDetails,
            IComparableItemDetails<IUnit> unitDetails,
            IComparableItemDetails<ICruiser> hullDetails)
        {
            Helper.AssertIsNotNull(buildingDetails, unitDetails, hullDetails);

            BuildingDetails = buildingDetails;
            UnitDetails = unitDetails;
            HullDetails = hullDetails;
        }
    }
}
