using BattleCruisers.Buildables;
using BattleCruisers.Buildables.Buildings.Turrets;
using BattleCruisers.Utils;
using NSubstitute;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BattleCruisers.TestScenes
{
	public class ArtilleryBarrelControllerTests : MonoBehaviour 
	{
		public GameObject targetGameObject;

		public TurretBarrelController left;
		public TurretBarrelController farLeft;
		public TurretBarrelController right;
		public TurretBarrelController farRight;

		void Start()
		{
			IFactionable target = Substitute.For<IFactionable>();
			target.GameObject.Returns(targetGameObject);

			left.Target = target;
			left.Initialise(Faction.Blues);

			farLeft.Target = target;
			farLeft.Initialise(Faction.Blues);

			right.Target = target;
			right.Initialise(Faction.Blues);

			farRight.Target = target;
			farRight.Initialise(Faction.Blues);
		}
	}
}
