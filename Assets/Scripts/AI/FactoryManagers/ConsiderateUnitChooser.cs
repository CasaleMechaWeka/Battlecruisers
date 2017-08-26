using System.Collections.Generic;
using System.Linq;
using BattleCruisers.Buildables;
using BattleCruisers.Buildables.Units;
using BattleCruisers.Drones;
using BattleCruisers.Utils;
using UnityEngine.Assertions;

// FELIX  Avoid duplicate code
namespace BattleCruisers.AI.FactoryManagers
{
	/// <summary>
    /// Only chooses units which will leave a specified number of drones free
    /// (hence "Considerate").
	/// 
	/// Updates the chosen unit every time the cruiser's number of drones changes.
	/// </summary>
	public class ConsiderateUnitChooser : IUnitChooser
	{
		private readonly IList<IBuildableWrapper<IUnit>> _units;
		private readonly IDroneManager _droneManager;
        private readonly int _spareDroneNum;

		public IBuildableWrapper<IUnit> ChosenUnit { get; private set; }

		public ConsiderateUnitChooser(IList<IBuildableWrapper<IUnit>> units, IDroneManager droneManager, int spareDroneNum)
		{
			Helper.AssertIsNotNull(units, droneManager);
			Assert.IsTrue(units.Count != 0);

			_units = units;
			_droneManager = droneManager;
            _spareDroneNum = spareDroneNum;

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
                    .Where(wrapper => wrapper.Buildable.NumOfDronesRequired + _spareDroneNum <= _droneManager.NumOfDrones)
					.OrderByDescending(wrapper => wrapper.Buildable.NumOfDronesRequired)
					.FirstOrDefault();
		}

		public void Dispose()
		{
			_droneManager.DroneNumChanged -= _droneManager_DroneNumChanged;
		}
	}
}
