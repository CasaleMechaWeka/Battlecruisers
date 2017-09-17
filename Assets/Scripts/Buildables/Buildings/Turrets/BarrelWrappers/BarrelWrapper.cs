using System.Collections.Generic;
using System.Linq;
using BattleCruisers.Buildables.Buildings.Turrets.AngleCalculators;
using BattleCruisers.Buildables.Buildings.Turrets.BarrelControllers;
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
        private BarrelController[] _barrels;
		private ITargetProcessorWrapper _targetProcessorWrapper;
        protected IFactoryProvider _factoryProvider;
        protected Faction _enemyFaction;
        protected IList<TargetType> _attackCapabilities;

        public Vector2 Position { get { return transform.position; } }
        public float DamagePerS { get; private set; }
        public float RangeInM { get; private set; }

        private List<Renderer> _renderers;
        public IList<Renderer> Renderers { get { return _renderers; } }

		private ITarget _target;
        public ITarget Target 
        { 
            get { return _target; }
            set 
            {
                _target = value;

                foreach (BarrelController barrel in _barrels)
                {
                    barrel.Target = _target;
                }
            }
        }

        private float _boostMultiplier;
        public float BoostMultiplier
        {
            get { return _boostMultiplier; }
            set
            {
                _boostMultiplier = value;

				foreach (BarrelController barrel in _barrels)
                {
                    barrel.TurretStats.BoostMultiplier = _boostMultiplier;
                }
            }
        }

        public virtual void StaticInitialise()
        {
            _renderers = new List<Renderer>();

            InitialiseBarrels();

            DamagePerS = _barrels.Sum(barrel => barrel.TurretStats.DamagePerS);
            RangeInM = _barrels.Max(barrel => barrel.TurretStats.rangeInM);

            _targetProcessorWrapper = gameObject.GetComponentInChildren<ITargetProcessorWrapper>();
            Assert.IsNotNull(_targetProcessorWrapper);
        }

        private void InitialiseBarrels()
        {
            _barrels = gameObject.GetComponentsInChildren<BarrelController>();
            Assert.IsTrue(_barrels.Length != 0);

            foreach (BarrelController barrel in _barrels)
            {
                barrel.StaticInitialise();
                _renderers.AddRange(barrel.Renderers);
            }
        }

        public void Initialise(IFactoryProvider factoryProvider, Faction enemyFaction, IList<TargetType> attackCapabilities)
        {
            Helper.AssertIsNotNull(factoryProvider, enemyFaction, attackCapabilities);

            _factoryProvider = factoryProvider;
            _enemyFaction = enemyFaction;
            _attackCapabilities = attackCapabilities;

			foreach (BarrelController barrel in _barrels)
            {
                InitialiseBarrelController(barrel);
            }
        }

        protected virtual void InitialiseBarrelController(BarrelController barrel)
        {
			barrel
				.Initialise(
                    CreateTargetFilter(), 
					CreateAngleCalculator(), 
                    CreateRotationMovementController(barrel));
        }

        public void StartAttackingTargets()
        {
            _targetProcessorWrapper.StartProvidingTargets(
                _factoryProvider.TargetsFactory,
                this,
                _enemyFaction,
                RangeInM,
                _attackCapabilities);
        }

        protected ITargetFilter CreateTargetFilter()
        {
            return _factoryProvider.TargetsFactory.CreateTargetFilter(_enemyFaction, _attackCapabilities);
        }

        protected abstract IAngleCalculator CreateAngleCalculator();

        protected virtual IRotationMovementController CreateRotationMovementController(BarrelController barrel)
        {
            return 
                _factoryProvider.MovementControllerFactory.CreateRotationMovementController(
                    barrel.TurretStats.turretRotateSpeedInDegrees, 
                    barrel.transform);
        }

        public void Dispose()
        {
            _targetProcessorWrapper.Dispose();
            _targetProcessorWrapper = null;
        }
    }
}
