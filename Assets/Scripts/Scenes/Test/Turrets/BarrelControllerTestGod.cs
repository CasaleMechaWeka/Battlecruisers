using BattleCruisers.Buildables;
using BattleCruisers.Buildables.Buildings.Turrets.AngleCalculators;
using BattleCruisers.Buildables.Buildings.Turrets.BarrelControllers;
using BattleCruisers.Projectiles.Stats;
using BattleCruisers.Scenes.Test.Utilities;
using NSubstitute;
using UnityEngine;

namespace BattleCruisers.Scenes.Test
{
    public abstract class BarrelControllerTestGod : TestGodBase
	{
		public GameObject targetGameObject;

        protected override void Start()
        {
            base.Start();

            Helper helper = new Helper();

            ITarget target = CreateTarget(targetGameObject.transform.position);

            BarrelController[] turretBarrels = FindObjectsOfType<BarrelController>();

            foreach (BarrelController barrel in turretBarrels)
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
