using BattleCruisers.Buildables;
using BattleCruisers.Buildables.Buildings;
using BattleCruisers.Buildables.Buildings.Factories;
using BattleCruisers.Projectiles.DamageAppliers;
using BattleCruisers.Projectiles.Stats;
using BattleCruisers.Scenes.Test.Utilities;
using BattleCruisers.Targets.TargetFinders.Filters;
using BattleCruisers.Utils.Threading;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace BattleCruisers.Scenes.Test.Projectiles
{
    public class DamageApplierTestGod : TestGodBase
    {
        private IBuilding _baseTarget;
        private TestAircraftController _aircraft;
        private IBuilding[] _targets;

        protected override List<GameObject> GetGameObjects()
        {
            _baseTarget = FindObjectOfType<AirFactory>();
			_aircraft = FindObjectOfType<TestAircraftController>();
            _targets = FindObjectsOfType<Building>();

            List<GameObject> gameObjects
                = _targets
                    .Select(target => target.GameObject)
                    .ToList();
            gameObjects.Add(_aircraft.GameObject);
            return gameObjects;
        }

        protected override void Setup(Helper helper)
        {
            Faction factionToTarget = Faction.Blues;

            // Setup targets
			_aircraft.UseDummyMovementController = true;
            helper.InitialiseUnit(_aircraft, factionToTarget);
            _aircraft.StartConstruction();

            foreach (IBuilding target in _targets)
            {
                helper.InitialiseBuilding(target, factionToTarget);
                target.StartConstruction();
            }

            // Setup damage applier
            IDamageStats damageStats = new DamageStats(damage: 50, damageRadiusInM: 5);
            ITargetFilter targetFilter = new DummyTargetFilter(isMatchResult: true);
            IDamageApplier damageApplier = new AreaOfEffectDamageApplier(damageStats, targetFilter);

            TimeScaleDeferrer deferrer = GetComponent<TimeScaleDeferrer>();
            deferrer.Defer(
                () => damageApplier.ApplyDamage(_baseTarget, _baseTarget.Position, damageSource: null),
                delayInS: 1);
	    }
    }
}
