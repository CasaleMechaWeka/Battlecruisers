using System.Collections.Generic;
using BattleCruisers.Buildables;
using BattleCruisers.Buildables.Units;
using BattleCruisers.Drones;

namespace BattleCruisers.AI.FactoryManagers
{
    /// <summary>
    /// Chooses the most expensive unit the cruiser can afford, while mainting
    /// a specified number of spare drones.
    /// </summary>
    public class ConsiderateUnitChooser : MostExpensiveUnitChooserBase
	{
        private readonly int _spareDroneNum;

		public ConsiderateUnitChooser(IList<IBuildableWrapper<IUnit>> units, IDroneManager droneManager, int spareDroneNum)
            :  base(units, droneManager)
		{
            _spareDroneNum = spareDroneNum;
		}

        protected override bool IsBuildableAcceptable(int buildableDroneNum, int droneManagerDroneNum)
        {
            return buildableDroneNum + _spareDroneNum <= droneManagerDroneNum;
        }
    }
}
