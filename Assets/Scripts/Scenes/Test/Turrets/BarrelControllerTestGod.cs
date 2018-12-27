using BattleCruisers.Buildables;
using BattleCruisers.Buildables.Buildings.Turrets.AngleCalculators;
using BattleCruisers.Buildables.Buildings.Turrets.BarrelControllers;
using BattleCruisers.Projectiles.Stats;
using BattleCruisers.Scenes.Test.Utilities;
using NSubstitute;
using UnityEngine;

namespace BattleCruisers.Scenes.Test
{
    public abstract class BarrelControllerTestGod : MonoBehaviour
	{
		public GameObject targetGameObject;

		void Start()
		{
            Helper helper = new Helper();

			ITarget target = Substitute.For<ITarget>();
			Vector2 targetPosition = targetGameObject.transform.position;
			target.Position.Returns(targetPosition);

            BarrelController[] turretBarrels = FindObjectsOfType<BarrelController>();

            foreach (BarrelController barrel in turretBarrels)
			{
				barrel.StaticInitialise();
				barrel.Target = target;

                IBarrelControllerArgs barrelControllerArgs
                    = helper.CreateBarrelControllerArgs(barrel, angleCalculator: CreateAngleCalculator(barrel.ProjectileStats));

                barrel.Initialise(barrelControllerArgs);
			}
		}

        protected abstract IAngleCalculator CreateAngleCalculator(IProjectileStats projectileStats);
	}
}
