using BattleCruisers.Buildables;
using BattleCruisers.Buildables.Buildings.Turrets;
using BattleCruisers.Scenes.Test.Utilities;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace BattleCruisers.Scenes.Test.Turrets.AntiShip
{
    public abstract class MortarTestGod : TestGodBase
    {
        private TurretController[] _mortars;
        private TestAircraftController _target;

        protected abstract List<Vector2> TargetPatrolPoints { get; }

        protected override List<GameObject> GetGameObjects()
        {
            _mortars = FindObjectsOfType<TurretController>();
            _target = FindObjectOfType<TestAircraftController>();

            List<GameObject> gameObjects
                = _mortars
                    .Select(mortar => mortar.GameObject)
                    .ToList();
            gameObjects.Add(_target.GameObject);
            return gameObjects;
        }

        protected override void Setup(Helper helper)
        {
            // Setup target
            _target.PatrolPoints = TargetPatrolPoints;
            _target.SetTargetType(TargetType.Ships);  // So mortars will attack this
            helper.InitialiseUnit(_target, Faction.Blues);
            _target.StartConstruction();

            // Setup mortars
            foreach (TurretController mortar in _mortars)
            {
                helper.InitialiseBuilding(mortar, Faction.Reds);
                mortar.StartConstruction();
            }
        }
    }
}
