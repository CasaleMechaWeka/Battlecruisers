using BattleCruisers.Buildables.Buildings;
using BattleCruisers.Buildables.Units;
using BattleCruisers.Cruisers;
using BattleCruisers.Utils.Fetchers;
using UnityEngine;
using UnityEngine.Assertions;

namespace BattleCruisers.UI.Common.BuildingDetails
{
    public class ItemDetailsGroupController : MonoBehaviour, IItemDetailsGroup
    {
        public IComparableItemDetails<IBuilding> BuildingDetails { get; private set; }
        public IComparableItemDetails<IUnit> UnitDetails { get; private set; }
        public IComparableItemDetails<ICruiser> HullDetails { get; private set; }

        public void Initialise(ISpriteProvider spriteProvider)
        {
            ComparableBuildingDetailsController buildingDetails = GetComponentInChildren<ComparableBuildingDetailsController>();
            Assert.IsNotNull(buildingDetails);
            buildingDetails.Initialise(spriteProvider);
            BuildingDetails = buildingDetails;

            ComparableUnitDetailsController unitDetails = GetComponentInChildren<ComparableUnitDetailsController>();
            Assert.IsNotNull(unitDetails);
            unitDetails.Initialise();
            UnitDetails = unitDetails;

            ComparableCruiserDetailsController cruiserDetails = GetComponentInChildren<ComparableCruiserDetailsController>();
            Assert.IsNotNull(cruiserDetails);
            cruiserDetails.Initialise();
            HullDetails = cruiserDetails;
        }
    }
}
