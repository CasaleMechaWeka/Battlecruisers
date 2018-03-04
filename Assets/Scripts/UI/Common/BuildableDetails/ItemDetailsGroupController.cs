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
            BuildingDetails = GetComponentInChildren<IComparableItemDetails<IBuilding>>();
            Assert.IsNotNull(BuildingDetails);

            UnitDetails = GetComponentInChildren<IComparableItemDetails<IUnit>>();
            Assert.IsNotNull(UnitDetails);

            HullDetails = GetComponentInChildren<IComparableItemDetails<ICruiser>>();
            Assert.IsNotNull(HullDetails);
        }
    }
}
