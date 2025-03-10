using BattleCruisers.Buildables;
using BattleCruisers.Buildables.Buildings.Turrets;
using BattleCruisers.Scenes.Test.Utilities;
using System.Collections.Generic;
using UnityEngine;

namespace BattleCruisers.Scenes.Test.Turrets.AntiShip
{
    public class AngleLimiterTestGod : TestGodBase
    {
        private TestAircraftController _target;
        private TurretController _turret;

        public List<Vector2> targetPatrolPoints;
        public TargetType targetType;

        protected override List<GameObject> GetGameObjects()
        {
            _target = FindObjectOfType<TestAircraftController>();
            _turret = FindObjectOfType<TurretController>();

            return new List<GameObject>()
            {
                _target.GameObject,
                _turret.GameObject
            };
        }

        protected override void Setup(Helper helper)
        {
            // Setup target
            _target.PatrolPoints = targetPatrolPoints;
            _target.SetTargetType(targetType);
            helper.InitialiseUnit(_target, Faction.Blues);
            _target.StartConstruction();


            // Setup turret
            helper.InitialiseBuilding(_turret, Faction.Reds);
            _turret.StartConstruction();
        }
    }
}
