using BattleCruisers.Buildables.Buildings;
using BattleCruisers.Buildables.Units;
using BattleCruisers.Cruisers;
using BattleCruisers.Utils;
using UnityEngine;

namespace BattleCruisers.UI.Common.BuildableDetails
{
    public class ItemDetailsGroupController : MonoBehaviour, IItemDetailsGroup
    {
        public BuildingDetailsController buildingDetails;
        public IComparableItemDetails<IBuilding> BuildingDetails => buildingDetails;

        public UnitDetailsController unitDetails;
        public IComparableItemDetails<IUnit> UnitDetails => unitDetails;

        public CruiserDetailsController cruiserDetails;
        public IComparableItemDetails<ICruiser> HullDetails => cruiserDetails;

        public void Initialise()
        {
            Helper.AssertIsNotNull(buildingDetails, unitDetails, cruiserDetails);

            buildingDetails.Initialise();
            buildingDetails.Hide();

            unitDetails.Initialise();
            unitDetails.Hide();

            cruiserDetails.Initialise();
            cruiserDetails.Hide();
        }
    }
}
