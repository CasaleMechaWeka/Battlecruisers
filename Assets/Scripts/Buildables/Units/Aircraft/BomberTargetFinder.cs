using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BattleCruisers.Units.Aircraft
{
	public interface ITargetFinder
	{
		GameObject FindTarget();
	}

	// FELIX  Implement properly :P
	public class BomberTargetFinder : MonoBehaviour, ITargetFinder
	{
		public GameObject tempTarget;

		public GameObject FindTarget()
		{
			return tempTarget;
		}
	}
}
