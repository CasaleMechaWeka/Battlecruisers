using BattleCruisers.Buildables;
using BattleCruisers.Buildables.Buildings.Turrets;
using BattleCruisers.Scenes.Test.Utilities;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace BattleCruisers.Scenes.Test.Effects.Deaths
{
    public class AircraftDestroyedTestGod : TestGodBase 
	{
        private TurretController[] _turrets;
        public TestAircraftController aircraftToDestroy, blockingAircraft;
		public List<Vector2> patrolPoints;

        protected override IList<GameObject> GetGameObjects()
        {
            _turrets = FindObjectsOfType<TurretController>();

            IList<GameObject> gameObjects
                = _turrets
                    .Select(turret => turret.GameObject)
                    .ToList();
            gameObjects.Add(aircraftToDestroy.GameObject);
            gameObjects.Add(blockingAircraft.GameObject);

            return gameObjects;
        }

        protected override void Setup(Helper helper)
        {
            Faction aircraftFaction = Faction.Blues;
            Faction turretFaction = Faction.Reds;

            // Initialise aircraft
			aircraftToDestroy.PatrolPoints = patrolPoints;
            helper.InitialiseUnit(aircraftToDestroy, aircraftFaction);
			aircraftToDestroy.StartConstruction();
            Invoke("DestroyAircraft", time: 2);

            blockingAircraft.UseDummyMovementController = true;
            helper.InitialiseUnit(blockingAircraft, turretFaction);
            blockingAircraft.StartConstruction();

            // Initialise turrets
            foreach (TurretController turret in _turrets)
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
