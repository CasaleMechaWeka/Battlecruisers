using BattleCruisers.Buildables.Buildings.Turrets;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BattleCruisers.Utils;

namespace BattleCruisers.TestScenes
{
	public class ArtilleryBarrelControllerTests : MonoBehaviour 
	{
		public GameObject target;

		public TurretBarrelController left;
		public TurretBarrelController farLeft;
		public TurretBarrelController right;
		public TurretBarrelController farRight;

		void Start()
		{
			Logging.Initialise();

			left.Target = target;
			farLeft.Target = target;
			right.Target = target;
			farRight.Target = target;
		}
	}
}
