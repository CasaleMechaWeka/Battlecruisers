using BattleCruisers.Buildables;
using BattleCruisers.Buildables.Buildings.Turrets.AngleCalculators;
using BattleCruisers.Buildables.Buildings.Turrets.BarrelControllers;
using BattleCruisers.Movement.Rotation;
using BattleCruisers.Scenes.Test.Utilities;
using BattleCruisers.Targets.TargetFinders.Filters;
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
			ITarget target = Substitute.For<ITarget>();
			Vector2 targetPosition = targetGameObject.transform.position;
			target.Position.Returns(targetPosition);

			ITargetFilter targetFilter = Substitute.For<ITargetFilter>();

            BuildableInitialisationArgs args = new BuildableInitialisationArgs(new Helper());

            BarrelController[] turretBarrels = FindObjectsOfType<BarrelController>();

            foreach (ShellTurretBarrelController barrel in turretBarrels)
			{
				barrel.StaticInitialise();
                IAngleCalculator angleCalculator = AngleCalculator;
				IRotationMovementController rotationMovementController = new RotationMovementController(angleCalculator, barrel.TurretStats.TurretRotateSpeedInDegrees, barrel.transform);
				barrel.Target = target;
                barrel.Initialise(targetFilter, angleCalculator, rotationMovementController, args.FactoryProvider);
			}
		}
	}
}
