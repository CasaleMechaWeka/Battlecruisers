using BattleCruisers.Buildables;
using BattleCruisers.Buildables.Buildings.Turrets.AngleCalculators;
using BattleCruisers.Buildables.Buildings.Turrets.BarrelControllers;
using BattleCruisers.Projectiles.Stats;
using BattleCruisers.Scenes.Test.Utilities;
using NSubstitute;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace BattleCruisers.Scenes.Test
{
    public abstract class BarrelControllerTestGod : TestGodBase
	{
        private BarrelController[] _turretBarrels;
        public GameObject targetGameObject;

        protected override List<GameObject> GetGameObjects()
        {
            _turretBarrels = FindObjectsOfType<BarrelController>();

            return
                _turretBarrels
                    .Select(barrel => barrel.gameObject)
                    .ToList();
        }

        protected override void Setup(Helper helper)
        {
            ITarget target = CreateTarget(targetGameObject.transform.position);

            foreach (BarrelController barrel in _turretBarrels)
			{
				barrel.StaticInitialise();
				barrel.Target = target;

                IBarrelControllerArgs barrelControllerArgs
                    = helper.CreateBarrelControllerArgs(
                        barrel, 
                        _updaterProvider.PerFrameUpdater,
                        angleCalculator: CreateAngleCalculator(barrel.ProjectileStats));

                barrel.InitialiseAsync(barrelControllerArgs);
			}
		}

        protected abstract IAngleCalculator CreateAngleCalculator(IProjectileStats projectileStats);

        protected virtual ITarget CreateTarget(Vector2 targetPosition)
        {
            ITarget target = Substitute.For<ITarget>();
            target.Position.Returns(targetPosition);
            return target;
        }
	}
}
