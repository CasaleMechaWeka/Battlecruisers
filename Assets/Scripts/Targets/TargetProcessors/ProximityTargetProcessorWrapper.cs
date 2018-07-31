using BattleCruisers.Targets.TargetFinders;
using BattleCruisers.Targets.TargetFinders.Filters;
using BattleCruisers.Targets.TargetProcessors.Ranking;
using UnityEngine.Assertions;

namespace BattleCruisers.Targets.TargetProcessors
{
    public class ProximityTargetProcessorWrapper : TargetProcessorWrapper
	{
		private ITargetFinder _targetFinder;
        private IHighestPriorityTargetTracker _targetTracker;

        protected override ITargetProcessor CreateTargetProcessor(ITargetProcessorArgs args, ITargetRanker targetRanker)
		{
            _targetFinder = CreateTargetFinder(args);
            _targetTracker = args.TargetsFactory.CreateHighestPriorityTargetTracker(_targetFinder, targetRanker);
            return args.TargetsFactory.CreateTargetProcessor(_targetTracker);
        }

        protected virtual ITargetFinder CreateTargetFinder(ITargetProcessorArgs args)
        {
			CircleTargetDetector enemyDetector = gameObject.GetComponentInChildren<CircleTargetDetector>();
			Assert.IsNotNull(enemyDetector);
			
			// Create target finder
			enemyDetector.Initialise(args.MaxRangeInM);
			ITargetFilter enemyDetectionFilter = args.TargetsFactory.CreateTargetFilter(args.EnemyFaction, args.AttackCapabilities);
			return args.TargetsFactory.CreateRangedTargetFinder(enemyDetector, enemyDetectionFilter);
        }

        protected override void CleanUp()
        {
            base.CleanUp();

            _targetTracker.DisposeManagedState();
            _targetFinder.DisposeManagedState();
        }
    }
}
