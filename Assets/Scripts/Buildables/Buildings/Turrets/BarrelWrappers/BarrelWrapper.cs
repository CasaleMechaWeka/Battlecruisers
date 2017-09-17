using System;
using System.Collections.Generic;
using BattleCruisers.Buildables.Buildings.Turrets.AngleCalculators;
using BattleCruisers.Buildables.Buildings.Turrets.BarrelControllers;
using BattleCruisers.Buildables.Buildings.Turrets.Stats;
using BattleCruisers.Movement.Rotation;
using BattleCruisers.Targets.TargetFinders.Filters;
using BattleCruisers.Targets.TargetProcessors;
using BattleCruisers.Utils;
using UnityEngine;
using UnityEngine.Assertions;

namespace BattleCruisers.Buildables.Buildings.Turrets.BarrelWrappers
{
    public abstract class BarrelWrapper : MonoBehaviour, IBarrelWrapper
    {
        protected BarrelController _barrelController;
        protected IFactoryProvider _factoryProvider;
        protected Faction _enemyFaction;
        protected IList<TargetType> _attackCapabilities;
        private ITargetProcessorWrapper _targetProcessorWrapper;

        // FELIX
		private TurretStats TurretStats { get { return _barrelController.TurretStats; } }

        public IList<Renderer> Renderers { get { return _barrelController.Renderers; } }
		public Vector2 Position { get { return transform.position; } }

        public ITarget Target 
        { 
            get { return _barrelController.Target; }
            set { _barrelController.Target = value; }
        }

        public float BoostMultiplier
        {
            get { return TurretStats.BoostMultiplier; }
            set { TurretStats.BoostMultiplier = value; }
        }

        // FELIX
        public float DamagePerS
        {
            get
            {
                return TurretStats.DamagePerS;
            }
        }

		// FELIX
		public float RangeInM
        {
            get
            {
                return TurretStats.rangeInM;
            }
        }

        public virtual void StaticInitialise()
        {
            _barrelController = gameObject.GetComponentInChildren<BarrelController>();
            Assert.IsNotNull(_barrelController);
            _barrelController.StaticInitialise();

            _targetProcessorWrapper = gameObject.GetComponentInChildren<ITargetProcessorWrapper>();
            Assert.IsNotNull(_targetProcessorWrapper);
        }

        public void Initialise(IFactoryProvider factoryProvider, Faction enemyFaction, IList<TargetType> attackCapabilities)
        {
            Helper.AssertIsNotNull(factoryProvider, enemyFaction, attackCapabilities);

            _factoryProvider = factoryProvider;
            _enemyFaction = enemyFaction;
            _attackCapabilities = attackCapabilities;

            InitialiseBarrelController();
        }

        protected virtual void InitialiseBarrelController()
        {
			_barrelController
				.Initialise(
                    CreateTargetFilter(), 
					CreateAngleCalculator(), 
					CreateRotationMovementController());
        }

        public void StartAttackingTargets()
        {
            _targetProcessorWrapper.StartProvidingTargets(
                _factoryProvider.TargetsFactory,
                this,
                _enemyFaction,
                TurretStats.rangeInM,
                _attackCapabilities);
        }

        protected ITargetFilter CreateTargetFilter()
        {
            return _factoryProvider.TargetsFactory.CreateTargetFilter(_enemyFaction, _attackCapabilities);
        }

        protected abstract IAngleCalculator CreateAngleCalculator();

        protected virtual IRotationMovementController CreateRotationMovementController()
        {
            return _factoryProvider.MovementControllerFactory.CreateRotationMovementController(
                _barrelController.TurretStats.turretRotateSpeedInDegrees, _barrelController.transform);
        }

        public void Dispose()
        {
            _targetProcessorWrapper.Dispose();
            _targetProcessorWrapper = null;
        }
    }
}
