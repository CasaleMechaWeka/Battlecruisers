using BattleCruisers.Buildables;
using BattleCruisers.Cruisers;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BattleCruisers.Targets.TargetFinders
{
	// FELIX  Implement properly :P
	public class BomberTargetFinder : MonoBehaviour, ITargetFinder
	{
		private Cruiser _enemyCruiser;

		public BomberTargetFinder(Cruiser enemyCruiser)
		{
			_enemyCruiser = enemyCruiser;
		}

		public IFactionable FindTarget()
		{
			return _enemyCruiser;
		}
	}
}
