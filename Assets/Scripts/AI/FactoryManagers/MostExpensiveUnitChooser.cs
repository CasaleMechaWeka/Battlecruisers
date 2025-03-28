using System.Collections.Generic;
using System.Linq;
using BattleCruisers.Buildables;
using BattleCruisers.Buildables.Units;
using BattleCruisers.Cruisers.Drones;
using BattleCruisers.Utils;
using UnityEngine.Assertions;

namespace BattleCruisers.AI.FactoryManagers
{
	/// <summary>
	/// Chooses the most expensive acceptable unit.
	/// 
	/// Updates the chosen unit every time the cruiser's number of drones changes.
	/// </summary>
	public class MostExpensiveUnitChooser : UnitChooser
	{
		private readonly IList<IBuildableWrapper<IUnit>> _units;
		private readonly IDroneManager _droneManager;

		public MostExpensiveUnitChooser(IList<IBuildableWrapper<IUnit>> units, IDroneManager droneManager)
		{
			Helper.AssertIsNotNull(units, droneManager);
			Assert.IsTrue(units.Count != 0);

			_units = units;
			_droneManager = droneManager;

			_droneManager.DroneNumChanged += _droneManager_DroneNumChanged;

			ChooseUnit();
		}

		private void _droneManager_DroneNumChanged(object sender, DroneNumChangedEventArgs e)
		{
			ChooseUnit();
		}

		private void ChooseUnit()
		{
			ChosenUnit =
				_units
					.Where(wrapper => wrapper.Buildable.NumOfDronesRequired <= _droneManager.NumOfDrones)
					.OrderByDescending(wrapper => wrapper.Buildable.NumOfDronesRequired)
					.FirstOrDefault();
		}

		public override void DisposeManagedState()
		{
			_droneManager.DroneNumChanged -= _droneManager_DroneNumChanged;
		}
	}
}
