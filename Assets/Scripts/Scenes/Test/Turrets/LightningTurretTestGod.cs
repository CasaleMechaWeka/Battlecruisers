using BattleCruisers.Buildables.Buildings.Factories;
using BattleCruisers.Buildables.Buildings.Turrets.BarrelControllers;
using BattleCruisers.Movement.Rotation;
using BattleCruisers.Scenes.Test.Utilities;
using BattleCruisers.Targets.TargetFinders.Filters;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using BCUtils = BattleCruisers.Utils;

namespace BattleCruisers.Scenes.Test.Turrets
{
    public class LightningTurretTestGod : TestGodBase
    {
        [SerializeField]
        private LightningBarrelController _lightningBarrel;
        [SerializeField]
        private AirFactory _airFactory;

        protected override List<GameObject> GetGameObjects()
        {
            BCUtils.Helper.AssertIsNotNull(_lightningBarrel, _airFactory);

            return new List<GameObject>()
            {
                _lightningBarrel.gameObject,
                _airFactory.GameObject
            };
        }

        protected override async Task SetupAsync(Helper helper)
        {
            // Setup target
            helper.InitialiseBuilding(_airFactory);
            
            // Setup lightning barrel
            _lightningBarrel.StaticInitialise();

            IBarrelControllerArgs barrelControllerArgs
                = helper.CreateBarrelControllerArgs(
                    _lightningBarrel,
                    _updaterProvider.PerFrameUpdater,
                    targetFilter: new DummyTargetFilter(isMatchResult: true),
                    rotationMovementController: new DummyRotationMovementController(isOnTarget: true));

            await _lightningBarrel.InitialiseAsync(barrelControllerArgs);
			_lightningBarrel.Target = _airFactory;
	    }
	}
}
