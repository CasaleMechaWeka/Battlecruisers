using BattleCruisers.Cruisers.Slots.Feedback;
using BattleCruisers.Utils.PlatformAbstractions;
using NSubstitute;
using NUnit.Framework;

namespace BattleCruisers.Tests.Cruisers.Slots.Feedback
{
    public class BoostFeedbackTests
    {
        private IBoostFeedback _feedback;
        private IGameObject _singleBoostEffect, _doubleBoostEffect;

        [SetUp]
        public void TestSetup()
        {
            _singleBoostEffect = Substitute.For<IGameObject>();
            _doubleBoostEffect = Substitute.For<IGameObject>();

            _feedback = new BoostFeedback(_singleBoostEffect, _doubleBoostEffect);

            _singleBoostEffect.ClearReceivedCalls();
            _singleBoostEffect.ClearReceivedCalls();
        }

        [Test]
        public void Constructor()
        {
            _feedback = new BoostFeedback(_singleBoostEffect, _doubleBoostEffect);

            _singleBoostEffect.Received().IsVisible = false;
            _doubleBoostEffect.Received().IsVisible = false;
        }

        [Test]
        public void SetState_Off()
        {
            _feedback.State = BoostState.Off;

            _singleBoostEffect.Received().IsVisible = false;
            _doubleBoostEffect.Received().IsVisible = false;
        }

        [Test]
        public void SetState_Single()
        {
            _feedback.State = BoostState.Single;

            _singleBoostEffect.Received().IsVisible = true;
            _doubleBoostEffect.Received().IsVisible = false;
        }

        [Test]
        public void SetState_Double()
        {
            _feedback.State = BoostState.Double;

            _singleBoostEffect.Received().IsVisible = false;
            _doubleBoostEffect.Received().IsVisible = true;
        }
    }
}