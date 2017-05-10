using BattleCruisers.Buildables;
using BattleCruisers.Buildables.Buildings.Turrets.BarrelControllers;
using BattleCruisers.Utils;
using NSubstitute;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BattleCruisers.TestScenes
{
	public class TurretBarrelControllerTests : MonoBehaviour 
	{
		public GameObject targetGameObject;

		public TurretBarrelController barrelBelowToLeft;
		public TurretBarrelController barrelBelowToRight;
		public TurretBarrelController barrelAboveToLeft;
		public TurretBarrelController barrelAboveToRight;

		public TurretBarrelController barrelBelowToLeftMirroed;
		public TurretBarrelController barrelBelowToRightMirroed;
		public TurretBarrelController barrelAboveToLeftMirroed;
		public TurretBarrelController barrelAboveToRightMirroed;

		void Start()
		{
			ITarget target = Substitute.For<ITarget>();
			target.GameObject.Returns(targetGameObject);

			// Turrets facing right
			barrelBelowToLeft.Target = target;
			barrelBelowToLeft.Initialise(Faction.Blues);
			barrelBelowToRight.Target = target;
			barrelBelowToRight.Initialise(Faction.Blues);
			barrelAboveToLeft.Target = target;
			barrelAboveToLeft.Initialise(Faction.Blues);
			barrelAboveToRight.Target = target;
			barrelAboveToRight.Initialise(Faction.Blues);

			// Turrets facing left (mirrored)
			barrelBelowToLeftMirroed.Target = target;
			barrelBelowToLeftMirroed.Initialise(Faction.Blues);
			barrelBelowToRightMirroed.Target = target;
			barrelBelowToRightMirroed.Initialise(Faction.Blues);
			barrelAboveToLeftMirroed.Target = target;
			barrelAboveToLeftMirroed.Initialise(Faction.Blues);
			barrelAboveToRightMirroed.Target = target;
			barrelAboveToRightMirroed.Initialise(Faction.Blues);
		}
	}
}
