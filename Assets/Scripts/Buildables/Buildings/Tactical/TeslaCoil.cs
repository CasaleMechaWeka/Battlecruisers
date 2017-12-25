using BattleCruisers.Buildables.Buildings.Turrets.BarrelControllers.FireInterval;
using BattleCruisers.Buildables.Buildings.Turrets.Stats;
using BattleCruisers.Targets;
using BattleCruisers.Targets.TargetFinders;
using BattleCruisers.Targets.TargetFinders.Filters;
using BattleCruisers.Targets.TargetProcessors;
using BattleCruisers.Targets.TargetProcessors.Ranking;
using BattleCruisers.Utils;
using UnityEngine.Assertions;

namespace BattleCruisers.Buildables.Buildings.Tactical
{
    public class TeslaCoil : Building, ITargetConsumer
	{
		private TeslaCoilStats _teslaCoilStats;
		private CircleTargetDetector _rocketDetector;
		private ITargetFinder _targetFinder;
		private ITargetProcessor _targetProcessor;
		private IFireIntervalManager _fireIntervalManager;

		public override TargetValue TargetValue { get { return TargetValue.Medium; } }
		public ITarget Target { get; set; }

		public override void StaticInitialise()
		{
			base.StaticInitialise();

			_teslaCoilStats = gameObject.GetComponent<TeslaCoilStats>();
			Assert.IsNotNull(_teslaCoilStats);
            _teslaCoilStats.Initialise();

			_rocketDetector = gameObject.GetComponentInChildren<CircleTargetDetector>();
			Assert.IsNotNull(_rocketDetector);

            FireIntervalManager fireIntervalManager = gameObject.GetComponent<FireIntervalManager>();
			Assert.IsNotNull(fireIntervalManager);
			fireIntervalManager.Initialise(_teslaCoilStats);
            _fireIntervalManager = fireIntervalManager;

			_attackCapabilities.Add(TargetType.Rocket);
		}

		protected override void OnBuildableCompleted()
		{
			base.OnBuildableCompleted();

			_rocketDetector.Initialise(_teslaCoilStats.rangeInM);
			Faction enemyFaction = Helper.GetOppositeFaction(Faction);
            ITargetFilter enemyDetectionFilter = _targetsFactory.CreateTargetFilter(enemyFaction, _attackCapabilities);
			_targetFinder = _targetsFactory.CreateRangedTargetFinder(_rocketDetector, enemyDetectionFilter);
			
			ITargetRanker targetRanker = _targetsFactory.CreateEqualTargetRanker();
			_targetProcessor = _targetsFactory.CreateTargetProcessor(_targetFinder, targetRanker);
			_targetProcessor.AddTargetConsumer(this);
            _targetProcessor.StartProcessingTargets();
		}

		protected override void OnUpdate()
		{
			base.OnUpdate();

			if (Target != null && _fireIntervalManager.ShouldFire())
			{
                Target.TakeDamage(_teslaCoilStats.Damage, damageSource: this);
                _fireIntervalManager.OnFired();
			}
		}

		protected override void OnDestroyed()
		{
			base.OnDestroyed();

			if (BuildableState == BuildableState.Completed)
			{
				_targetProcessor.RemoveTargetConsumer(this);
				_targetProcessor.Dispose();
				_targetProcessor = null;

				_targetFinder.Dispose();
				_targetFinder = null;
			}
		}
	}
}
