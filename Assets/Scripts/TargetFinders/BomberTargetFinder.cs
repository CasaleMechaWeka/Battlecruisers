using BattleCruisers.Buildables;
using BattleCruisers.Cruisers;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BattleCruisers.TargetFinders
{
	// FELIX  Implement properly :P
	public class BomberTargetFinder : MonoBehaviour, ITargetFinder
	{
		private Cruiser _enemyCruiser;

		public event EventHandler<TargetEventArgs> TargetFound;
		public event EventHandler<TargetEventArgs> TargetLost;

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
