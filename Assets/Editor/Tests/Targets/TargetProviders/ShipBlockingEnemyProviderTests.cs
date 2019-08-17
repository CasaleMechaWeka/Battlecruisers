using BattleCruisers.Buildables;
using BattleCruisers.Buildables.Units;
using BattleCruisers.Targets;
using BattleCruisers.Targets.Factories;
using BattleCruisers.Targets.TargetDetectors;
using BattleCruisers.Targets.TargetFinders;
using BattleCruisers.Targets.TargetFinders.Filters;
using BattleCruisers.Targets.TargetProcessors;
using BattleCruisers.Targets.TargetProviders;
using BattleCruisers.Targets.TargetTrackers;
using BattleCruisers.Targets.TargetTrackers.Ranking;
using BattleCruisers.Utils.Factories;
using NSubstitute;
using NUnit.Framework;
using UnityAsserts = UnityEngine.Assertions;

namespace BattleCruisers.Tests.Targets.TargetProviders
{
    public class ShipBlockingEnemyProviderTests
    {
        private ShipBlockingEnemyProvider _targetProvider;
        private ITargetConsumer _asTargetConsumer;
        private ITargetFilter _isInFrontFilter;
        private ITarget _target;

        [SetUp]
        public void SetuUp()
        {
            UnityAsserts.Assert.raiseExceptions = true;

            _isInFrontFilter = Substitute.For<ITargetFilter>();
            _target = Substitute.For<ITarget>();

            ITargetDetector enemyDetector = Substitute.For<ITargetDetector>();
            ITargetFilter enemyFilter = Substitute.For<ITargetFilter>();
            ITargetFinder enemyFinder = Substitute.For<ITargetFinder>();
            ITargetRanker enemyRanker = Substitute.For<ITargetRanker>();
            IRankedTargetTracker targetTracker = Substitute.For<IRankedTargetTracker>();
			ITargetProcessor targetProcessor = Substitute.For<ITargetProcessor>();
            IUnit parentUnit = Substitute.For<IUnit>();

            ITargetFactoriesProvider targetFactories = Substitute.For<ITargetFactoriesProvider>();
            targetFactories.FilterFactory.CreateTargetInFrontFilter(parentUnit).Returns(_isInFrontFilter);
            targetFactories.FilterFactory.CreateTargetFilter(default, targetTypes: null).ReturnsForAnyArgs(enemyFilter);
            targetFactories.FinderFactory.CreateRangedTargetFinder(enemyDetector, enemyFilter).Returns(enemyFinder);
            targetFactories.RankerFactory.EqualTargetRanker.Returns(enemyRanker);

            ICruiserSpecificFactories cruiserSpecificFactories = Substitute.For<ICruiserSpecificFactories>();
            cruiserSpecificFactories.Targets.TrackerFactory.CreateRankedTargetTracker(enemyFinder, enemyRanker).Returns(targetTracker);
            cruiserSpecificFactories.Targets.ProcessorFactory.CreateTargetProcessor(targetTracker).Returns(targetProcessor);

            _targetProvider = new ShipBlockingEnemyProvider(cruiserSpecificFactories, targetFactories, enemyDetector, parentUnit);
            _asTargetConsumer = _targetProvider;

            targetProcessor.Received().AddTargetConsumer(_targetProvider);
        }

        [Test]
        public void Constructor_TargetIsNull()
        {
            Assert.IsNull(_targetProvider.Target);
        }

        [Test]
        public void TargetAssigned_IsNotInFront_Throws()
        {
            _isInFrontFilter.IsMatch(_target).Returns(false);
            Assert.Throws<UnityAsserts.AssertionException>(() => _asTargetConsumer.Target = _target);
        }

        [Test]
        public void TargetAssigned_Null_DoesNotCheckIsInFront()
        {
            _asTargetConsumer.Target = null;
            _isInFrontFilter.DidNotReceive().IsMatch(element: null);
        }

        [Test]
        public void TargetAssigned_UpdatesProvidedTarget()
        {
            _isInFrontFilter.IsMatch(_target).Returns(true);
            _asTargetConsumer.Target = _target;
            Assert.AreSame(_target, _targetProvider.Target);
        }
    }
}
