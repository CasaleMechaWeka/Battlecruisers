using BattleCruisers.Buildables;
using BattleCruisers.Cruisers;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BattleCruisers.TargetFinders
{
	public interface ITargetFinder
	{
		bool IsTargetAvailable { get; }

		event EventHandler TargetFound;

		/// <returns>A target faction object, or null if no valid targets can be found.</returns>
		IFactionable FindTarget();
	}

	// FELIX  Implement properly :P
	public class BomberTargetFinder : MonoBehaviour, ITargetFinder
	{
		private Cruiser _enemyCruiser;

		public event EventHandler TargetFound;

		public bool IsTargetAvailable { get { return true; } }

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
