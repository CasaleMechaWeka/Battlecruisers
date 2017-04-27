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

		/// <returns>A target game object, or null if no valid targets can be found.</returns>
		GameObject FindTarget();
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

		public GameObject FindTarget()
		{
			return _enemyCruiser.gameObject;
		}
	}
}
