using BattleCruisers.Buildables.Buildings.Turrets.BarrelControllers.FireInterval;
using BattleCruisers.Buildables.Buildings.Turrets.BarrelControllers.FireInterval.States;
using NSubstitute;
using NUnit.Framework;

namespace BattleCruisers.Tests.Buildables.Buildings.Turrets.BarrelControllers.FireInterval.States
{
    public abstract class StatesTestBase
    {
        protected IState _otherState;
        protected IDurationProvider _durationProvider;
        protected float _timePassedInS;

        [SetUp]
        public virtual void TestSetup()
        {
            _otherState = Substitute.For<IState>();
            _durationProvider = Substitute.For<IDurationProvider>();
            _timePassedInS = 17.71f;
        }
    }
}
