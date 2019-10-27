using System.Collections.Generic;
using System.Linq;
using BattleCruisers.Buildables;
using BattleCruisers.Buildables.Buildings;
using BattleCruisers.Buildables.Buildings.Factories;
using BattleCruisers.Buildables.Buildings.Turrets;
using BattleCruisers.Scenes.Test.Utilities;
using UnityEngine;

namespace BattleCruisers.Scenes.Test.Naval
{
    public class ShipWithDefensiveNotClosestTargetTestGod : ShipStopsForEnemyCruiserTestGod
	{
        private TurretController[] _turrets;
        private IBuilding _navalFactory;

        protected override IList<GameObject> GetGameObjects()
        {
            // FELIX  Change return type to concrete type :P
            List<GameObject> gameObjects = (List<GameObject>)base.GetGameObjects();

            _turrets = FindObjectsOfType<TurretController>();
            IList<GameObject> turretGameObjects
                = _turrets
                    .Select(turret => turret.GameObject)
                    .ToList();
            gameObjects.AddRange(turretGameObjects);

            _navalFactory = FindObjectOfType<NavalFactory>();
            gameObjects.Add(_navalFactory.GameObject);

            return gameObjects;
        }

        protected override void Setup(Helper helper)
        {
            base.Setup(helper);

            // Turrets
            foreach (TurretController turret in _turrets)
            {
                helper.InitialiseBuilding(turret, Faction.Reds);
                turret.StartConstruction();
			}

            // Non turret target
            helper.InitialiseBuilding(_navalFactory, Faction.Reds);
            _navalFactory.StartConstruction();
        }
	}
}
