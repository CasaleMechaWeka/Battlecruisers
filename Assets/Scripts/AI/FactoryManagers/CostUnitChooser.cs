using System.Collections.Generic;
using System.Linq;
using BattleCruisers.Buildables;
using BattleCruisers.Buildables.Units;
using BattleCruisers.Drones;
using BattleCruisers.Utils;
using UnityEngine.Assertions;

namespace BattleCruisers.AI.FactoryManagers
{
    // FELIX  Rename!

	/// <summary>
	/// Chooses the most expensive acceptable unit.
	/// 
	/// Updates the chosen unit every time the cruiser's number of drones changes.
	/// </summary>
	public abstract class CostUnitChooser : IUnitChooser
	{
		private readonly IList<IBuildableWrapper<IUnit>> _units;
		private readonly IDroneManager _droneManager;

		public IBuildableWrapper<IUnit> ChosenUnit { get; private set; }

		public CostUnitChooser(IList<IBuildableWrapper<IUnit>> units, IDroneManager droneManager)
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

        protected abstract bool IsBuildableAcceptable(int buildableDroneNum, int droneManagerDroneNum);

		public void Dispose()
		{
			_droneManager.DroneNumChanged -= _droneManager_DroneNumChanged;
		}
	}
}
