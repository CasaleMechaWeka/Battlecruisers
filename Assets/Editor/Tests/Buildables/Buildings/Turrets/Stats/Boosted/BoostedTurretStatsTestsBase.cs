using BattleCruisers.Buildables;
using BattleCruisers.Buildables.Boost;
using BattleCruisers.Buildables.Boost.GlobalProviders;
using BattleCruisers.Buildables.Buildings.Turrets.Stats;
using NSubstitute;
using NUnit.Framework;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace BattleCruisers.Tests.Buildables.Buildings.Turrets.Stats.Boosted
{
    public abstract class BoostedTurretStatsTestsBase<TStats> where TStats : class, IBasicTurretStats
    {
        protected TStats _baseStats;
        protected IBoostFactory _boostFactory;
        protected IGlobalBoostProviders _globalBoostProviders;
        protected ObservableCollection<IBoostProvider> _localBoostProviders;
        protected IBoostable _boostable;
        protected IBoostableGroup _boostableGroup;

        [SetUp]
        public virtual void SetuUp()
        {
            _baseStats = Substitute.For<TStats>();

            ReadOnlyCollection<TargetType> attackCapabilities = new ReadOnlyCollection<TargetType>(new List<TargetType>());
            _baseStats.AttackCapabilities.Returns(attackCapabilities);
            _baseStats.DurationInS.Returns(0.2f);
            _baseStats.FireRatePerS.Returns(0.3f);
            _baseStats.MeanFireRatePerS.Returns(0.4f);
            _baseStats.MinRangeInM.Returns(0.5f);
            _baseStats.RangeInM.Returns(0.6f);

            _boostFactory = Substitute.For<IBoostFactory>();

            _boostable = Substitute.For<IBoostable>();
            _boostable.BoostMultiplier.Returns(3.3f);
            _boostFactory.CreateBoostable().Returns(_boostable);

            _boostableGroup = Substitute.For<IBoostableGroup>();
            _boostFactory.CreateBoostableGroup().Returns(_boostableGroup);

            _globalBoostProviders = new GlobalBoostProviders();

            _localBoostProviders = new ObservableCollection<IBoostProvider>();
        }
    }
}
