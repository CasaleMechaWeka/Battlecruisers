using BattleCruisers.Buildables;
using BattleCruisers.Buildables.Buildings.Turrets;
using BattleCruisers.Scenes.Test.Utilities;
using System.Collections.Generic;
using UnityEngine;

namespace BattleCruisers.Scenes.Test.Turrets.AntiShip
{
    public class AngleLimiterTestGod : TestGodBase
    {
        public List<Vector2> targetPatrolPoints;
        public TargetType targetType;

        protected override void Start()
        {
            base.Start();

            Helper helper = new Helper(updaterProvider: _updaterProvider);


            // Setup target
            TestAircraftController target = FindObjectOfType<TestAircraftController>();
            target.PatrolPoints = targetPatrolPoints;
            target.SetTargetType(targetType);
            helper.InitialiseUnit(target, Faction.Blues);
            target.StartConstruction();


            // Setup turret
            TurretController turret = FindObjectOfType<TurretController>();
            helper.InitialiseBuilding(turret, Faction.Reds);
            turret.StartConstruction();
        }
    }
}
