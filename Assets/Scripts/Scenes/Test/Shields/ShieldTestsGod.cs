using BattleCruisers.Buildables;
using BattleCruisers.Buildables.Buildings.Tactical.Shields;
using BattleCruisers.Buildables.Buildings.Turrets.BarrelControllers;
using BattleCruisers.Scenes.Test.Utilities;
using BattleCruisers.Targets.TargetFinders.Filters;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;

namespace BattleCruisers.Scenes.Test.Shields
{
    public class ShieldTestsGod : TestGodBase 
	{
        private ShieldGenerator _shield;
        private BarrelController _turret;

        protected override List<GameObject> GetGameObjects()
        {
            _shield = FindObjectOfType<ShieldGenerator>();
            Assert.IsNotNull(_shield);

            _turret = FindObjectOfType<BarrelController>();
            Assert.IsNotNull(_turret);

            return new List<GameObject>()
            {
                _shield.GameObject,
                _turret.gameObject
            };
        }

        protected override void Setup(Helper helper)
        {
            // Setup shield
            helper.InitialiseBuilding(_shield, Faction.Reds);
            _shield.StartConstruction();

            // Setup turret
            _turret.StaticInitialise();

            IList<TargetType> targetTypes = new List<TargetType>() { TargetType.Buildings };
            ITargetFilter targetFilter = new FactionAndTargetTypeFilter(_shield.Faction, targetTypes);

            IBarrelControllerArgs barrelControllerArgs
                = helper
                    .CreateBarrelControllerArgs(
                    _turret,
                    _updaterProvider.PerFrameUpdater,
                    targetFilter);

            _turret.InitialiseAsync(barrelControllerArgs);
			_turret.Target = _shield;
		}
	}
}
