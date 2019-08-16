using System.Collections.Generic;
using BattleCruisers.Buildables;
using BattleCruisers.Buildables.Buildings.Turrets;
using BattleCruisers.Scenes.Test.Utilities;
using UnityEngine;

namespace BattleCruisers.Scenes.Test.Turrets.AntiShip
{
    public abstract class MortarTestGod : TestGodBase
    {
        protected abstract List<Vector2> TargetPatrolPoints { get; }

        protected override void Start()
        {
            base.Start();

            Helper helper = new Helper(updaterProvider: _updaterProvider);


            // Setup target
            TestAircraftController target = FindObjectOfType<TestAircraftController>();
            target.PatrolPoints = TargetPatrolPoints;
            target.SetTargetType(TargetType.Ships);  // So mortars will attack this
            helper.InitialiseUnit(target, Faction.Blues);
            target.StartConstruction();


            // Setup mortars
            TurretController[] mortars = FindObjectsOfType<TurretController>();
            foreach (TurretController mortar in mortars)
            {
                helper.InitialiseBuilding(mortar, Faction.Reds);
                mortar.StartConstruction();
            }
        }
    }
}
