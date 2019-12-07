using BattleCruisers.Buildables.Buildings.Factories;
using BattleCruisers.Buildables.Buildings.Turrets.AngleCalculators;
using BattleCruisers.Buildables.Buildings.Turrets.BarrelControllers;
using BattleCruisers.Scenes.Test.Utilities;
using BattleCruisers.Targets.TargetFinders.Filters;
using BattleCruisers.Utils.BattleScene.Update;
using BattleCruisers.Utils.Threading;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.Assertions;

namespace BattleCruisers.Scenes.Test
{
    public class BroadsidesBarrelTestGod : TestGodBase
	{
        private Factory _target;
        private BarrelController _doubleBarrel;

        protected override async Task<Helper> CreateHelperAsync(IUpdaterProvider updaterProvider)
        {
            IDeferrer deferrer = GetComponent<IDeferrer>();
            Assert.IsNotNull(deferrer);

            return await HelperFactory.CreateHelperAsync(deferrer: deferrer, updaterProvider: updaterProvider);
        }

        protected override List<GameObject> GetGameObjects()
        {
            _target = FindObjectOfType<Factory>();
            _doubleBarrel = FindObjectOfType<BarrelController>();

            return new List<GameObject>()
            {
                _target.GameObject,
                _doubleBarrel.gameObject
            };
        }

        protected override void Setup(Helper helper)
        {
            // Initialise target
            helper.InitialiseBuilding(_target);

            // Initialise double barrel
			_doubleBarrel.StaticInitialise();
			_doubleBarrel.Target = _target;

            IBarrelControllerArgs barrelControllerArgs
                = helper.CreateBarrelControllerArgs(
                    _doubleBarrel,
                    _updaterProvider.PerFrameUpdater,
                    targetFilter: new ExactMatchTargetFilter() { Target = _target },
                    angleCalculator: new ArtilleryAngleCalculator(new AngleHelper(), new AngleConverter(), _doubleBarrel.ProjectileStats));

            _doubleBarrel.InitialiseAsync(barrelControllerArgs);
		}
	}
}
