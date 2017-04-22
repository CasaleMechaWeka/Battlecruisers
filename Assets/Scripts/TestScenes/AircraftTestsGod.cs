using BattleCruisers.Buildables.Units;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BattleCruisers.TestScenes
{
	public class AircraftTestsGod : MonoBehaviour 
	{
		public BomberController bomber;

		void Start() 
		{
			IList<Vector2> patrolPoints = new List<Vector2> { new Vector2(-5, 3), new Vector2(5, 3) };
			bomber.Initialise(patrolPoints);
		}
	}
}
