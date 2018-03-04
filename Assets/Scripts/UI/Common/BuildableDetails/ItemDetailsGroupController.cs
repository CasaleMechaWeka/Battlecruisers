using BattleCruisers.Buildables.Buildings;
using BattleCruisers.Buildables.Units;
using BattleCruisers.Cruisers;
using UnityEngine;
using UnityEngine.Assertions;

namespace BattleCruisers.UI.Common.BuildingDetails
{
    public class ItemDetailsGroupController : MonoBehaviour, IItemDetailsGroup
    {
        public IComparableItemDetails<IBuilding> BuildingDetails { get; private set; }
        public IComparableItemDetails<IUnit> UnitDetails { get; private set; }
        public IComparableItemDetails<ICruiser> HullDetails { get; private set; }

        public void Initialise()
        {
            BuildingDetails = GetComponent<IComparableItemDetails<IBuilding>>();
            Assert.IsNotNull(BuildingDetails);

            UnitDetails = GetComponent<IComparableItemDetails<IUnit>>();
            Assert.IsNotNull(UnitDetails);

            HullDetails = GetComponent<IComparableItemDetails<ICruiser>>();
            Assert.IsNotNull(HullDetails);
        }
    }
}
