using BattleCruisers.UI.Cameras.Targets;
using BattleCruisers.UI.Cameras.Targets.Providers;
using NSubstitute;
using NUnit.Framework;
using System.Collections.Generic;
using UnityEngine;

namespace BattleCruisers.Tests.UI.Cameras.Targets.Providers
{
    public class CompositeCameraTargetProviderTests
    {
        private ICameraTargetProvider _compositeTargetProvider;
        private IStaticCameraTargetProvider _defaultTargetProvider;
        private IUserInputCameraTargetProvider _highPriorityTargetProvider, _lowPriorityTargetProvider;
        private ICameraTarget _defaultTarget, _highPriorityTarget, _lowPriorityTarget;
        private int _targetChangedCount;

        [SetUp]
        public void TestSetup()
        {
            _defaultTargetProvider = Substitute.For<IStaticCameraTargetProvider>();
            _highPriorityTargetProvider = Substitute.For<IUserInputCameraTargetProvider>();
            _lowPriorityTargetProvider = Substitute.For<IUserInputCameraTargetProvider>();

            IList<IUserInputCameraTargetProvider> providers = new List<IUserInputCameraTargetProvider>()
            {
                _highPriorityTargetProvider,
                _lowPriorityTargetProvider
            };

            _compositeTargetProvider
                = new CompositeCameraTargetProvider(
                    _defaultTargetProvider,
                    providers);

            _targetChangedCount = 0;
            _compositeTargetProvider.TargetChanged += (sender, e) => _targetChangedCount++;

            _defaultTarget = Substitute.For<ICameraTarget>();
            _defaultTargetProvider.Target.Returns(_defaultTarget);
            _defaultTargetProvider.Priority.Returns(1);

            _highPriorityTarget = Substitute.For<ICameraTarget>();
            _highPriorityTargetProvider.Target.Returns(_highPriorityTarget);
            _highPriorityTargetProvider.Priority.Returns(3);

            _lowPriorityTarget = Substitute.For<ICameraTarget>();
            _lowPriorityTargetProvider.Target.Returns(_lowPriorityTarget);
            _lowPriorityTargetProvider.Priority.Returns(2);
        }

        [Test]
        public void InitialState()
        {
            Assert.AreSame(_defaultTarget, _compositeTargetProvider.Target);
        }

        [Test]
        public void UserInputStarted_HigherPriorityThanCurrent()
        {
            _highPriorityTargetProvider.UserInputStarted += Raise.Event();
            Assert.AreEqual(_highPriorityTarget, _compositeTargetProvider.Target);
        }

        [Test]
        public void UserInputStarted_LowerPriorityThanCurrent()
        {
            // Default provider has lowest priority, so first need to replace with higher priority provider
            _highPriorityTargetProvider.UserInputStarted += Raise.Event();
            Assert.AreEqual(_highPriorityTarget, _compositeTargetProvider.Target);

            // Now can show that lower priority provider is ignored
            _lowPriorityTargetProvider.UserInputStarted += Raise.Event();
            Assert.AreEqual(_highPriorityTarget, _compositeTargetProvider.Target);
        }

        [Test]
        public void UserInputEnded_WhileNotActiveProvider()
        {
            _highPriorityTargetProvider.UserInputEnded += Raise.Event();
        }

        [Test]
        public void UserInputEnded_WhileActiveProvider()
        {
            _highPriorityTargetProvider.UserInputStarted += Raise.Event();
            Vector2 targetCenterPosition = new Vector2(1, 2);

            _highPriorityTargetProvider.UserInputEnded += Raise.Event();
        }

        [Test]
        public void _activeTargetProvider_TargetChanged()
        {
            _defaultTargetProvider.TargetChanged += Raise.Event();
            Assert.AreEqual(1, _targetChangedCount);
        }
    }
}