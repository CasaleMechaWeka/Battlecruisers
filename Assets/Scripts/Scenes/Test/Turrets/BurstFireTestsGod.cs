using BattleCruisers.Buildables;
using BattleCruisers.Buildables.Buildings.Turrets.BarrelControllers;
using BattleCruisers.Utils;
using NSubstitute;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BattleCruisers.Scenes.Test
{
	public class BurstFireTestsGod : MonoBehaviour 
	{
		public TurretBarrelController barrel1, barrel2, barrel3;
		public GameObject target1, target2, target3;

		void Start()
		{
			InitialisePair(barrel1, target1);
			InitialisePair(barrel2, target2);
			InitialisePair(barrel3, target3);
		}

		private void InitialisePair(TurretBarrelController barrel, GameObject targetGameObject)
		{
			ITarget target = Substitute.For<ITarget>();
			target.GameObject.Returns(targetGameObject);
			barrel.Target = target;
			barrel.Initialise(Faction.Blues);
		}
	}
}
