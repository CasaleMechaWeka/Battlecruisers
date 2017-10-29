using BattleCruisers.Buildables;
using BattleCruisers.Buildables.Buildings.Turrets.AngleCalculators;
using BattleCruisers.Buildables.Buildings.Turrets.BarrelControllers;
using BattleCruisers.Movement.Predictors;
using BattleCruisers.Movement.Rotation;
using BattleCruisers.Scenes.Test.Utilities;
using BattleCruisers.Targets.TargetFinders.Filters;
using NSubstitute;
using UnityEngine;

namespace BattleCruisers.Scenes.Test
{
    public class BurstFireTestsGod : MonoBehaviour 
	{
		private ITargetFilter _targetFilter;
        private ITargetPositionPredictor _targetPositionPredictor;
		private IAngleCalculator _angleCalculator;
        private IRotationHelper _rotationHelper;
        private Helper _helper;
        public BarrelController barrel1, barrel2, barrel3;
		public GameObject target1, target2, target3;

		void Start()
		{
            _targetPositionPredictor = new DummyTargetPositionpredictor();
            _angleCalculator = new AngleCalculator();
            _rotationHelper = new RotationHelper();
			_targetFilter = Substitute.For<ITargetFilter>();
            _helper = new Helper();

			InitialisePair(barrel1, target1);
			InitialisePair(barrel2, target2);
			InitialisePair(barrel3, target3);
		}

        private void InitialisePair(BarrelController barrel, GameObject targetGameObject)
		{
			barrel.StaticInitialise();
			
            ITarget target = Substitute.For<ITarget>();
			Vector2 targetPosition = targetGameObject.transform.position;
			target.Position.Returns(targetPosition);
			barrel.Target = target;
			
            IRotationMovementController rotationMovementController = new RotationMovementController(_rotationHelper, barrel.TurretStats.TurretRotateSpeedInDegrees, barrel.transform);
            BuildableInitialisationArgs args = new BuildableInitialisationArgs(_helper);

            barrel.Initialise(_targetFilter, _targetPositionPredictor, _angleCalculator, rotationMovementController, args.FactoryProvider);
		}
	}
}
