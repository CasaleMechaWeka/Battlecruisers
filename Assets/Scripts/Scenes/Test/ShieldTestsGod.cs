using BattleCruisers.Buildables;
using BattleCruisers.Buildables.Buildings.Tactical;
using BattleCruisers.Buildables.Buildings.Turrets.AngleCalculators;
using BattleCruisers.Buildables.Buildings.Turrets.BarrelControllers;
using BattleCruisers.Movement.Predictors;
using BattleCruisers.Targets.TargetFinders.Filters;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BattleCruisers.Scenes.Test
{
	public class ShieldTestsGod : MonoBehaviour 
	{
		public ShieldController shield;
		public TurretBarrelController turret;

		void Start () 
		{
			shield.Initialise(Faction.Reds);

			ITargetFilter targetFilter = new TargetFilter(shield.Faction, TargetType.Buildings);
			IAngleCalculator angleCalculator = new AngleCalculator(new TargetPositionPredictorFactory());
			turret.Initialise(targetFilter, angleCalculator);
			turret.Target = shield;
		}
	}
}
