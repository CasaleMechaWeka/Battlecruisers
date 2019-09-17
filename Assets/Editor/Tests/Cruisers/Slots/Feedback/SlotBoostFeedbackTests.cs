using BattleCruisers.Buildables.Boost;
using BattleCruisers.Cruisers.Slots.Feedback;
using BattleCruisers.Utils.PlatformAbstractions.UI;
using NSubstitute;
using NUnit.Framework;
using System;
using System.Collections.ObjectModel;

namespace BattleCruisers.Tests.Cruisers.Slots.Feedback
{
    public class SlotBoostFeedbackTests
    {
#pragma warning disable CS0414  // Variable is assigned but never used
        private SlotBoostTextFeedback _slotBoostFeedback;
#pragma warning restore CS0414  // Variable is assigned but never used
        private ITextMesh _textMesh;
        private ObservableCollection<IBoostProvider> _boostProviders;
        private IBoostProvider _boostProvider;

        [SetUp]
        public void TestSetup()
        {
            _textMesh = Substitute.For<ITextMesh>();
            _boostProviders = new ObservableCollection<IBoostProvider>();

            _slotBoostFeedback = new SlotBoostTextFeedback(_textMesh, _boostProviders);

            _boostProvider = Substitute.For<IBoostProvider>();
        }

        [Test]
        public void InitialState()
        {
            _textMesh.Received().SetActive(isActive: false);
        }

        [Test]
        public void OneBoostProvider()
        {
            _textMesh.ClearReceivedCalls();

            _boostProviders.Add(_boostProvider);

            _textMesh.Received().SetActive(isActive: true);
            Assert.AreEqual("+", _textMesh.Text);
        }

        [Test]
        public void TwoBoostProviders()
        {
            _textMesh.ClearReceivedCalls();

            // First boost provider
            _boostProviders.Add(_boostProvider);

            _textMesh.Received().SetActive(isActive: true);
            Assert.AreEqual("+", _textMesh.Text);

            // Second boost provider
            _boostProviders.Add(_boostProvider);

            _textMesh.Received().SetActive(isActive: true);
            Assert.AreEqual("++", _textMesh.Text);
        }

        [Test]
        public void ThreeBoostProviders_Throws()
        {
            TwoBoostProviders();

            Assert.Throws<ArgumentException>(() => _boostProviders.Add(_boostProvider));
        }

        [Test]
        public void NoBoostProviders()
        {
            _textMesh.ClearReceivedCalls();

            // First boost provider
            _boostProviders.Add(_boostProvider);

            _textMesh.Received().SetActive(isActive: true);
            Assert.AreEqual("+", _textMesh.Text);

            // Remove boost provider
            _boostProviders.Remove(_boostProvider);

            _textMesh.Received().SetActive(isActive: false);
        }
    }
}