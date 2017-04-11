using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BattleCruisers.Buildables.Units.Detectors
{
	public class FriendDetector : FactionObjectDetector 
	{
		public Faction OwnFaction { private get; set; }

		protected override bool ShouldTriggerOnEntered(FactionObject factionObject)
		{
			return ShouldTrigger(factionObject);
		}

		protected override bool ShouldTriggerOnExited(FactionObject factionObject)
		{
			return ShouldTrigger(factionObject);
		}

		private bool ShouldTrigger(FactionObject factionObject)
		{
			return factionObject.Faction == OwnFaction;}
	}
}