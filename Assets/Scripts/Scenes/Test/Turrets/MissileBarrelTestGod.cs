using BattleCruisers.Buildables;
using BattleCruisers.Buildables.Buildings.Factories;
using BattleCruisers.Buildables.Buildings.Turrets.AngleCalculators;
using BattleCruisers.Buildables.Buildings.Turrets.BarrelControllers;
using BattleCruisers.Scenes.Test.Utilities;
using BattleCruisers.Targets.TargetFinders.Filters;
using System.Collections.Generic;
using UnityEngine;

namespace BattleCruisers.Scenes.Test.Turrets
{
    public class MissileBarrelTestGod : TestGodBase 
    {
        private AirFactory _target;
        private MissileBarrelController _missileBarrel;

        protected override List<GameObject> GetGameObjects()
        {
            _target = FindObjectOfType<AirFactory>();
            _missileBarrel = FindObjectOfType<MissileBarrelController>();

            return new List<GameObject>()
            {
                _target.GameObject,
                _missileBarrel.gameObject
            };
        }

        protected override void Setup(Helper helper)
        {
            // Setup target
            helper.InitialiseBuilding(_target, Faction.Reds);
            _target.StartConstruction();

            // Setup missile barrel controller
            IList<TargetType> targetTypes = new List<TargetType>() { TargetType.Buildings };
            ITargetFilter targetFilter = new FactionAndTargetTypeFilter(Faction.Reds, targetTypes);

            _missileBarrel.StaticInitialise();
			_missileBarrel.Target = _target;

            IBarrelControllerArgs barrelControllerArgs
                = helper.CreateBarrelControllerArgs(
                    _missileBarrel, 
                    _updaterProvider.PerFrameUpdater,
                    targetFilter: targetFilter,
                    angleCalculator: new StaticAngleCalculator(new AngleHelper(), desiredAngleInDegrees: 90));

            _missileBarrel.InitialiseAsync(barrelControllerArgs);
	    }
	}
}
