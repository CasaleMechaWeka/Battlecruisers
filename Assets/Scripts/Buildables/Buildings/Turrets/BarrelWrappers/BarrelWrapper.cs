using System.Collections.Generic;
using BattleCruisers.Buildables.Buildings.Turrets.AngleCalculators;
using BattleCruisers.Buildables.Buildings.Turrets.BarrelControllers;
using BattleCruisers.Buildables.Buildings.Turrets.Stats;
using BattleCruisers.Movement;
using BattleCruisers.Movement.Rotation;
using BattleCruisers.Targets;
using BattleCruisers.Targets.TargetFinders.Filters;
using UnityEngine;
using UnityEngine.Assertions;

namespace BattleCruisers.Buildables.Buildings.Turrets.BarrelWrappers
{
    public abstract class BarrelWrapper : MonoBehaviour, ITargetConsumer
    {
        protected BarrelController _barrelController;

        protected abstract IList<TargetType> AttackCapabilities { get; }
		public TurretStats TurretStats { get { return _barrelController.TurretStats; } }

        public ITarget Target 
        { 
            get { return _barrelController.Target; }
            set { _barrelController.Target = value; }
        }

        public void StaticInitialise()
        {
            _barrelController = gameObject.GetComponentInChildren<BarrelController>();
            Assert.IsNotNull(_barrelController);
            _barrelController.StaticInitialise();
        }

        public void Initialise(ITargetsFactory targetsFactory, IMovementControllerFactory movementControllerFactory, Faction enemyFaction)
        {
            _barrelController
                .Initialise(
                    CreateTargetFilter(targetsFactory, enemyFaction), 
                    CreateAngleCalculator(), 
                    CreateRotationMovementController(movementControllerFactory));
        }

        protected virtual ITargetFilter CreateTargetFilter(ITargetsFactory targetsFactory, Faction enemyFaction)
        {
            return targetsFactory.CreateTargetFilter(enemyFaction, AttackCapabilities);
        }

        protected abstract IAngleCalculator CreateAngleCalculator();

        protected virtual IRotationMovementController CreateRotationMovementController(IMovementControllerFactory movementControllerFactory)
        {
            return movementControllerFactory.CreateRotationMovementController(_barrelController.TurretStats.turretRotateSpeedInDegrees, _barrelController.transform);
        }
    }
}
