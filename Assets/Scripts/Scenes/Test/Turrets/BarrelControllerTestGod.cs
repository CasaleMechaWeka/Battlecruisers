using BattleCruisers.Buildables;
using BattleCruisers.Buildables.Buildings.Turrets.AngleCalculators;
using BattleCruisers.Buildables.Buildings.Turrets.BarrelControllers;
using BattleCruisers.Movement.Rotation;
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

			BarrelController[] turretBarrels = FindObjectsOfType<BarrelController>();

            foreach (BarrelController barrel in turretBarrels)
			{
				barrel.StaticInitialise();
				IRotationMovementController rotationMovementController = new RotationMovementController(AngleCalculator, barrel.TurretStats.turretRotateSpeedInDegrees, barrel.transform);
				barrel.Target = target;
				barrel.Initialise(targetFilter, AngleCalculator, rotationMovementController);
			}
		}
	}
}
