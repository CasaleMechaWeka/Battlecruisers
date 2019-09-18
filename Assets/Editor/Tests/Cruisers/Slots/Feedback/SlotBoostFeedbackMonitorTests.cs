using BattleCruisers.Buildables.Boost;
using BattleCruisers.Cruisers.Slots;
using BattleCruisers.Cruisers.Slots.Feedback;
using NSubstitute;
using NUnit.Framework;
using System.Collections.ObjectModel;

namespace BattleCruisers.Tests.Cruisers.Slots.Feedback
{
    public class SlotBoostFeedbackMonitorTests
    {
        private SlotBoostFeedbackMonitor _feedbackMonitor;
        private ISlot _parentSlot;
        private IBoostStateFinder _boostStateFinder;
        private IBoostFeedback _boostFeedback;
        private ObservableCollection<IBoostProvider> _slotBoostProviders;

        [SetUp]
        public void TestSetup()
        {
            _parentSlot = Substitute.For<ISlot>();
            _slotBoostProviders = new ObservableCollection<IBoostProvider>();
            _parentSlot.BoostProviders.Returns(_slotBoostProviders);

            _boostStateFinder = Substitute.For<IBoostStateFinder>();
            _boostFeedback = Substitute.For<IBoostFeedback>();

            _feedbackMonitor = new SlotBoostFeedbackMonitor(_parentSlot, _boostStateFinder, _boostFeedback);
        }

        [Test]
        public void SlotBuildingChanged()
        {
            _boostStateFinder
                .FindState(0, _parentSlot.Building.Value)
                .Returns(BoostState.Off);

            _parentSlot.Building.ValueChanged += Raise.Event();

            AssertFoundState(BoostState.Off);
        }

        [Test]
        public void SlotBoostProvidersChanged()
        {
            _boostStateFinder
                .FindState(1, _parentSlot.Building.Value)
                .Returns(BoostState.Single);


            IBoostProvider boostProvider = Substitute.For<IBoostProvider>();
            _slotBoostProviders.Add(boostProvider);

            AssertFoundState(BoostState.Single);
        }

        private void AssertFoundState(BoostState state)
        {
            _boostStateFinder.Received().FindState(_slotBoostProviders.Count, _parentSlot.Building.Value);
            _boostFeedback.Received().State = state;
        }
    }
}