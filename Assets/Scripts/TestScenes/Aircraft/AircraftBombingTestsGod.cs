using BattleCruisers.Buildables.Units;
using BattleCruisers.Utils;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace BattleCruisers.TestScenes.Aircraft
{
	public class AircraftBombingTestsGod : MonoBehaviour 
	{
		public BomberController bomber;
		public GameObject target;

		void Start() 
		{
			Logging.Initialise();

			bomber.Target = target;
		}
	}
}
