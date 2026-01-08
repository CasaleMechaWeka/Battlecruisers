using System.Collections.Generic;
using System.Linq;
using BattleCruisers.Buildables;
using BattleCruisers.Buildables.Units;
using BattleCruisers.Cruisers.Drones;
using BattleCruisers.Utils;
using UnityEngine;
using UnityEngine.Assertions;

namespace BattleCruisers.AI.FactoryManagers
{
	/// <summary>
	/// Cycles through a predefined unit list, but only selects units that are currently affordable
	/// (NumOfDronesRequired <= current drones). This creates an "escalating assortment" naturally
	/// as drones increase, while ensuring we don't get stuck on "most expensive only".
	///
	/// On each unit completion, advances to the next affordable unit (wrapping).
	/// Also re-evaluates when drone count changes.
	/// </summary>
	public class CyclingAffordableUnitChooser : UnitChooser
	{
		private readonly IList<IBuildableWrapper<IUnit>> _unitsInCycleOrder;
		private readonly DroneManager _droneManager;
		private int _cycleIndex;

		public CyclingAffordableUnitChooser(IList<IBuildableWrapper<IUnit>> unitsInCycleOrder, DroneManager droneManager)
		{
			Helper.AssertIsNotNull(unitsInCycleOrder, droneManager);
			Assert.IsTrue(unitsInCycleOrder.Count != 0);

			_unitsInCycleOrder = unitsInCycleOrder;
			_droneManager = droneManager;
			_cycleIndex = 0;

			_droneManager.DroneNumChanged += _droneManager_DroneNumChanged;

			// Initial pick
			ChooseNextAffordable(startFromCurrentIndex: true);
		}

		private void _droneManager_DroneNumChanged(object sender, DroneNumChangedEventArgs e)
		{
			// If current chosen unit is no longer affordable (shouldn't happen), or if we have none, pick again.
			if (ChosenUnit == null || ChosenUnit.Buildable.NumOfDronesRequired > _droneManager.NumOfDrones)
			{
				ChooseNextAffordable(startFromCurrentIndex: true);
			}
		}

		public override void OnUnitBuilt()
		{
			ChooseNextAffordable(startFromCurrentIndex: false);
		}

		private void ChooseNextAffordable(bool startFromCurrentIndex)
		{
			int start = startFromCurrentIndex ? _cycleIndex : (_cycleIndex + 1) % _unitsInCycleOrder.Count;

			// Try a full loop to find an affordable unit.
			for (int i = 0; i < _unitsInCycleOrder.Count; i++)
			{
				int idx = (start + i) % _unitsInCycleOrder.Count;
				IBuildableWrapper<IUnit> candidate = _unitsInCycleOrder[idx];
				if (candidate != null && candidate.Buildable.NumOfDronesRequired <= _droneManager.NumOfDrones)
				{
					_cycleIndex = idx;
					ChosenUnit = candidate;
					return;
				}
			}

			// None affordable yet: pick the cheapest (lowest drone requirement) so we at least queue something.
			IBuildableWrapper<IUnit> fallback =
				_unitsInCycleOrder
					.Where(u => u != null)
					.OrderBy(u => u.Buildable.NumOfDronesRequired)
					.FirstOrDefault();

			if (fallback != null)
			{
				_cycleIndex = _unitsInCycleOrder.IndexOf(fallback);
				ChosenUnit = fallback;
			}
		}

		public override void DisposeManagedState()
		{
			_droneManager.DroneNumChanged -= _droneManager_DroneNumChanged;
		}
	}
}


