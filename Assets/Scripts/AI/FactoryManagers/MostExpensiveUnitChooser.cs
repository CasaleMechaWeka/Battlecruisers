using System.Collections.Generic;
using BattleCruisers.Buildables;
using BattleCruisers.Buildables.Units;
using BattleCruisers.Drones;

namespace BattleCruisers.AI.FactoryManagers
{
    /// <summary>
    /// Chooses the most expensive unit the cruiser can afford (has enough drones for).
    /// </summary>
    public class MostExpensiveUnitChooser : CostUnitChooser
	{
        public MostExpensiveUnitChooser(IList<IBuildableWrapper<IUnit>> units, IDroneManager droneManager)
            : base(units, droneManager)
        {
		}

        protected override bool IsBuildableAcceptable(int buildableDroneNum, int droneManagerDroneNum)
        {
            return buildableDroneNum <= droneManagerDroneNum;
        }
    }
}
