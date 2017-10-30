using BattleCruisers.Buildables;
using BattleCruisers.Buildables.Buildings.Turrets.AngleCalculators;
using BattleCruisers.Buildables.Buildings.Turrets.BarrelControllers;
using BattleCruisers.Scenes.Test.Utilities;
using NSubstitute;
using UnityEngine;

namespace BattleCruisers.Scenes.Test
{
    public abstract class BarrelControllerTestGod : MonoBehaviour
	{
		public GameObject targetGameObject;

        protected abstract IAngleCalculator AngleCalculator { get; }

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
                    = helper.CreateBarrelControllerArgs(barrel, angleCalculator: AngleCalculator);

                barrel.Initialise(barrelControllerArgs);
			}
		}
	}
}
