using BattleCruisers.Buildables;
using BattleCruisers.Buildables.Buildings.Factories;
using BattleCruisers.Buildables.BuildProgress;
using BattleCruisers.Utils.PlatformAbstractions;
using BattleCruisers.Utils.PlatformAbstractions.UI;
using NSubstitute;
using NUnit.Framework;
using UnityAsserts = UnityEngine.Assertions;

namespace BattleCruisers.Tests.Buildables.BuildProgress
{
    public class BuildProgressFeedbackTests
    {
        private IBuildProgressFeedback _buildProgress;
        private IFillableImage _fillableImage;
        private IGameObject _pausedFeedback;
        private IBuildable _buildable1, _buildable2;
        private IFactory _factory;

        [SetUp]
        public void TestSetup()
        {
            _fillableImage = Substitute.For<IFillableImage>();
            _pausedFeedback = Substitute.For<IGameObject>();
            _buildProgress = new BuildProgressFeedback(_fillableImage, _pausedFeedback);

            _buildable1 = Substitute.For<IBuildable>();
            _buildable1.BuildableState.Returns(BuildableState.InProgress);
            _buildable1.BuildProgress.Returns(0.25f);

            _buildable2 = Substitute.For<IBuildable>();
            _buildable2.BuildableState.Returns(BuildableState.InProgress);
            _buildable2.BuildProgress.Returns(0.75f);

            _factory = Substitute.For<IFactory>();
            _factory.IsUnitPaused.Value.Returns(true);

            // Needed otherwise tests fail
            UnityAsserts.Assert.raiseExceptions = true;
        }

        [Test]
        public void InitialState_Hidden()
        {
            Assert.IsFalse(_fillableImage.IsVisible);
            Assert.IsFalse(_pausedFeedback.IsVisible);
        }

        #region ShowBuildProgress
        [Test]
        public void ShowBuildProgress_NullBuildable_Throws()
        {
            Assert.Throws<UnityAsserts.AssertionException>(() => _buildProgress.ShowBuildProgress(buildable: null, buildableFactory: _factory));
        }

        [Test]
        public void ShowBuildProgress_NullFactory_Throws()
        {
            Assert.Throws<UnityAsserts.AssertionException>(() => _buildProgress.ShowBuildProgress(_buildable1, buildableFactory: null));
        }

        [Test]
        public void ShowBuildProgress_CompletedBuildable_Throws()
        {
            _buildable1.BuildableState.Returns(BuildableState.Completed);
            Assert.Throws<UnityAsserts.AssertionException>(() => _buildProgress.ShowBuildProgress(_buildable1, _factory));
        }

        [Test]
        public void ShowBuildProgress_ValidBuildable_ShowsProgress()
        {
            _buildProgress.ShowBuildProgress(_buildable1, _factory);

            Assert.IsTrue(_fillableImage.IsVisible);
            Assert.AreEqual(1 - _buildable1.BuildProgress, _fillableImage.FillAmount);
            Assert.AreEqual(_factory.IsUnitPaused.Value, _pausedFeedback.IsVisible);
        }

        [Test]
        public void ShowBuildProgress_ValidBuildable_ReplacesCurrentBuilding_AndShowsProgress()
        {
            // First buildable
            _buildProgress.ShowBuildProgress(_buildable1, _factory);
            Assert.AreEqual(1 - _buildable1.BuildProgress, _fillableImage.FillAmount);

            // Second buildable
            _buildProgress.ShowBuildProgress(_buildable2, _factory);
            Assert.AreEqual(1 - _buildable2.BuildProgress, _fillableImage.FillAmount);
            AssertUnsubribedFromBuildable(_buildable1);
        }
        #endregion ShowBuildProgress

        [Test]
        public void BuildProgressChanged_UpdatesProgress()
        {
            _buildProgress.ShowBuildProgress(_buildable1, _factory);

            _buildable1.BuildProgress.Returns(0.55f);
            _buildable1.BuildableProgress += Raise.EventWith(new BuildProgressEventArgs(_buildable1));

            Assert.AreEqual(1 - _buildable1.BuildProgress, _fillableImage.FillAmount);
        }

        [Test]
        public void IsUnitPausedChanged_UpdatesPausedFeedbackVisibility()
        {
            _buildProgress.ShowBuildProgress(_buildable1, _factory);

            _factory.IsUnitPaused.Value.Returns(false);
            _factory.IsUnitPaused.ValueChanged += Raise.Event();
            Assert.IsFalse(_pausedFeedback.IsVisible);

            _factory.IsUnitPaused.Value.Returns(true);
            _factory.IsUnitPaused.ValueChanged += Raise.Event();
            Assert.IsTrue(_pausedFeedback.IsVisible);
        }

        [Test]
        public void BuildableCompleted_ResetsProgress()
        {
            _buildProgress.ShowBuildProgress(_buildable1, _factory);

            _buildable1.CompletedBuildable += Raise.Event();

            Assert.IsTrue(_fillableImage.IsVisible);
            Assert.AreEqual(1, _fillableImage.FillAmount);
        }

        [Test]
        public void BuidlableDestroyed_ResetsProgress()
        {
            _buildProgress.ShowBuildProgress(_buildable1, _factory);

            _buildable1.Destroyed += Raise.EventWith(new DestroyedEventArgs(_buildable1));

            Assert.IsTrue(_fillableImage.IsVisible);
            Assert.AreEqual(1, _fillableImage.FillAmount);
        }

        [Test]
        public void HideBuildProgress_HidesProgress_AndUnsubsribes()
        {
            _buildProgress.ShowBuildProgress(_buildable1, _factory);
            _buildProgress.HideBuildProgress();

            Assert.IsFalse(_fillableImage.IsVisible);
            Assert.IsFalse(_pausedFeedback.IsVisible);
            AssertUnsubribedFromBuildable(_buildable1);
            AssertUnsubsribedFromFactory(_factory);
        }

        private void AssertUnsubribedFromBuildable(IBuildable buildable)
        {
            // IBuildable events unsubsribed
            _fillableImage.ClearReceivedCalls();
            buildable.BuildableProgress += Raise.EventWith(new BuildProgressEventArgs(buildable));
            _fillableImage.DidNotReceiveWithAnyArgs().FillAmount = default;
        }

        private void AssertUnsubsribedFromFactory(IFactory factory)
        {
            _pausedFeedback.ClearReceivedCalls();
            factory.IsUnitPaused.ValueChanged += Raise.Event();
            _pausedFeedback.DidNotReceiveWithAnyArgs().IsVisible = default;
        }
    }
}