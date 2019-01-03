using BattleCruisers.Buildables;
using BattleCruisers.Targets.TargetFinders.Filters;
using NSubstitute;
using NUnit.Framework;

namespace BattleCruisers.Tests.Targets.TargetFinders.Filters
{
    public class MultipleExactMatchesTargetFilterTests
    {
        private IExactMatchTargetFilter _filter;
        private ITarget _target1, _target2;

        [SetUp]
        public void TestSetup()
        {
            _filter = new MultipleExactMatchesTargetFilter();

            _target1 = Substitute.For<ITarget>();
            _target2 = Substitute.For<ITarget>();
        }

        [Test]
        public void NoMatches()
        {
            Assert.IsFalse(_filter.IsMatch(_target1));
            Assert.IsFalse(_filter.IsMatch(_target2));
        }

        [Test]
        public void SingleMatch()
        {
            _filter.Target = _target1;

            Assert.IsTrue(_filter.IsMatch(_target1));
            Assert.IsFalse(_filter.IsMatch(_target2));
        }

        [Test]
        public void MultipleMatches()
        {
            _filter.Target = _target1;
            _filter.Target = _target2;

            Assert.IsTrue(_filter.IsMatch(_target1));
            Assert.IsTrue(_filter.IsMatch(_target2));
        }
    }
}