using BattleCruisers.Buildables;
using BattleCruisers.Targets;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BattleCruisers.Units.Aircraft
{
	public class FighterController : AircraftController, ITargetConsumer
	{
		public ITarget Target { private get; set; }
	}
}
