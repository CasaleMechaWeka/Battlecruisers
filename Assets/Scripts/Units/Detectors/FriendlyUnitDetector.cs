using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BattleCruisers.Units.Detectors
{
	public class FriendlyUnitDetector : UnitDetector 
	{
		public Faction OwnFaction { private get; set; }

		protected override bool ShouldTriggerOnEntered(Unit unit)
		{
			return ShouldTrigger(unit);
		}

		protected override bool ShouldTriggerOnExited(Unit unit)
		{
			return ShouldTrigger(unit);
		}

		private bool ShouldTrigger(Unit unit)
		{
			return unit.faction == OwnFaction;
		}
	}
}