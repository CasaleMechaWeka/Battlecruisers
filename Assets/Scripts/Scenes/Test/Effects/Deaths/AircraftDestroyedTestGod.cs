using System.Collections.Generic;
using BattleCruisers.Buildables;
using BattleCruisers.Buildables.Buildings.Turrets;
using BattleCruisers.Scenes.Test.Utilities;
using UnityEngine;

namespace BattleCruisers.Scenes.Test.Effects.Deaths
{
    public class AircraftDestroyedTestGod : TestGodBase 
	{
        public TestAircraftController aircraftToDestroy, blockingAircraft;
		public List<Vector2> patrolPoints;

		protected override void Start() 
		{
            base.Start();

			Helper helper = new Helper(updaterProvider: _updaterProvider);

            Faction aircraftFaction = Faction.Blues;
            Faction turretFaction = Faction.Reds;

            // Initialise aircraft
			aircraftToDestroy.PatrolPoints = patrolPoints;
            helper.InitialiseUnit(aircraftToDestroy, aircraftFaction);
			aircraftToDestroy.StartConstruction();
            Invoke("DestroyAircraft", time: 1);

            blockingAircraft.UseDummyMovementController = true;
            helper.InitialiseUnit(blockingAircraft, turretFaction);
            blockingAircraft.StartConstruction();

            // Initialise turrets
            TurretController[] turrets = FindObjectsOfType<TurretController>();

            foreach (TurretController turret in turrets)
            {
                helper.InitialiseBuilding(turret, turretFaction);
                turret.StartConstruction();
            }
		}

        private void DestroyAircraft()
        {
            aircraftToDestroy.TakeDamage(damageAmount: aircraftToDestroy.MaxHealth, damageSource: null);
        }
	}
}
