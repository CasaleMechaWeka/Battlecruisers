using BattleCruisers.Buildables.Buildings.Turrets;
using BattleCruisers.Utils;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BattleCruisers.TestScenes
{
	public class TurretBarrelControllerTests : MonoBehaviour 
	{
		public TurretBarrelController barrel1;
		public GameObject target1;

		void Start () 
		{
			Logging.Initialise();

			barrel1.Target = target1;
		}
	}
}
