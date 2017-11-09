using System.Collections.Generic;
using BattleCruisers.Buildables;
using BattleCruisers.Buildables.Buildings.Turrets;
using BattleCruisers.Scenes.Test.Utilities;
using UnityEngine;

namespace BattleCruisers.Scenes.Test.Turrets.AntiShip
{
    public class AngleLimiterTestGod : MonoBehaviour
    {
        public List<Vector2> targetPatrolPoints;
        public TargetType targetType;

        void Start()
        {
            Helper helper = new Helper();


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
