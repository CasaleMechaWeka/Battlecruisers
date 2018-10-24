using BattleCruisers.Buildables.Buildings;
using BattleCruisers.Buildables.Units;
using BattleCruisers.Cruisers;
using UnityEngine;
using UnityEngine.Assertions;

namespace BattleCruisers.UI.Common.BuildableDetails
{
    public class ItemDetailsGroupController : MonoBehaviour, IItemDetailsGroup
    {
        public IComparableItemDetails<IBuilding> BuildingDetails { get; private set; }
        public IComparableItemDetails<IUnit> UnitDetails { get; private set; }
        public IComparableItemDetails<ICruiser> HullDetails { get; private set; }

        public void Initialise()
        {
            ComparableBuildingDetailsController buildingDetails = GetComponentInChildren<ComparableBuildingDetailsController>();
            Assert.IsNotNull(buildingDetails);
            buildingDetails.Initialise();
            BuildingDetails = buildingDetails;
            BuildingDetails.Hide();

            ComparableUnitDetailsController unitDetails = GetComponentInChildren<ComparableUnitDetailsController>();
            Assert.IsNotNull(unitDetails);
            unitDetails.Initialise();
            UnitDetails = unitDetails;
            UnitDetails.Hide();

            ComparableCruiserDetailsController cruiserDetails = GetComponentInChildren<ComparableCruiserDetailsController>();
            Assert.IsNotNull(cruiserDetails);
            cruiserDetails.Initialise();
            HullDetails = cruiserDetails;
            HullDetails.Hide();
        }
    }
}
