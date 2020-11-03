using BattleCruisers.Buildables.Buildings.Factories;
using BattleCruisers.Buildables.Buildings.Turrets.BarrelControllers;
using BattleCruisers.Movement.Rotation;
using BattleCruisers.Scenes.Test.Utilities;
using BattleCruisers.Targets.TargetFinders.Filters;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

namespace BattleCruisers.Scenes.Test.Turrets
{
    public class LaserTurretTestGod : TestGodBase
    {
        private LaserBarrelController _laserBarrel;
        private AirFactory _airFactory;

        protected override List<GameObject> GetGameObjects()
        {
            _laserBarrel = FindObjectOfType<LaserBarrelController>();
            _airFactory = FindObjectOfType<AirFactory>();

            return new List<GameObject>()
            {
                _laserBarrel.gameObject,
                _airFactory.GameObject
            };
        }

        protected override async Task SetupAsync(Helper helper)
        {
            // Setup target
            helper.InitialiseBuilding(_airFactory);
            _airFactory.Destroyed += (sender, e) => _laserBarrel.Target = null;
            
            // Setup laser barrel
            _laserBarrel.StaticInitialise();

            IBarrelControllerArgs barrelControllerArgs
                = helper.CreateBarrelControllerArgs(
                    _laserBarrel,
                    _updaterProvider.PerFrameUpdater,
                    targetFilter: new DummyTargetFilter(isMatchResult: true),
                    rotationMovementController: new DummyRotationMovementController(isOnTarget: true));

            await _laserBarrel.InitialiseAsync(barrelControllerArgs);
			_laserBarrel.Target = _airFactory;
	    }
	}
}
