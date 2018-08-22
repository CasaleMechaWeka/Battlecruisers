// FELIX  :D
//using BattleCruisers.Buildables;
//using BattleCruisers.Buildables.BuildProgress;
//using BattleCruisers.Utils.PlatformAbstractions.UI;
//using NSubstitute;
//using NUnit.Framework;
//using UnityAsserts = UnityEngine.Assertions;

//namespace BattleCruisers.Tests.Buildables.BuildProgress
//{
//    public class BuildProgressFeedbackTests
//    {
//        private IBuildProgressFeedback _buildProgress;
//        private IFillableImage _fillableImage;
//        private IBuildable _buildable1, _buildable2;

//        [SetUp]
//        public void TestSetup()
//        {
//            _fillableImage = Substitute.For<IFillableImage>();
//            _buildProgress = new BuildProgressFeedback(_fillableImage);

//            _buildable1 = Substitute.For<IBuildable>();
//            _buildable1.BuildableState.Returns(BuildableState.InProgress);
//            _buildable1.BuildProgress.Returns(0.25f);

//            _buildable2 = Substitute.For<IBuildable>();
//            _buildable2.BuildableState.Returns(BuildableState.InProgress);
//            _buildable2.BuildProgress.Returns(0.75f);

//            UnityAsserts.Assert.raiseExceptions = true;
//        }

//        [Test]
//        public void InitialState_Hidden()
//        {
//            Assert.IsFalse(_fillableImage.IsVisible);
//        }

//        #region ShowBuildProgress
//        [Test]
//        public void ShowBuildProgress_NullBuildable_Throws()
//        {
//            Assert.Throws<UnityAsserts.AssertionException>(() => _buildProgress.ShowBuildProgress(buildable: null));
//        }

//        [Test]
//        public void ShowBuildProgress_CompletedBuildable_Throws()
//        {
//            _buildable1.BuildableState.Returns(BuildableState.Completed);
//            Assert.Throws<UnityAsserts.AssertionException>(() => _buildProgress.ShowBuildProgress(_buildable1));
//        }

//        [Test]
//        public void ShowBuildProgress_ValidBuildable_ShowsProgress()
//        {
//            _buildProgress.ShowBuildProgress(_buildable1);

//            Assert.IsTrue(_fillableImage.IsVisible);
//            Assert.AreEqual(1 - _buildable1.BuildProgress, _fillableImage.FillAmount);
//        }

//        [Test]
//        public void ShowBuildProgress_ValidBuildable_ReplacesCurrentBuilding_AndShowsProgress()
//        {
//            // First buildable
//            _buildProgress.ShowBuildProgress(_buildable1);
//            Assert.AreEqual(1 - _buildable1.BuildProgress, _fillableImage.FillAmount);

//            // Second buildable
//            _buildProgress.ShowBuildProgress(_buildable2);
//            Assert.AreEqual(1 - _buildable2.BuildProgress, _fillableImage.FillAmount);
//            AssertAreUnsubribed(_buildable1);
//        }
//        #endregion ShowBuildProgress

//        [Test]
//        public void BuildProgressChanged_UpdatesProgress()
//        {
//            _buildProgress.ShowBuildProgress(_buildable1);

//            _buildable1.BuildProgress.Returns(0.55f);
//            _buildable1.BuildableProgress += Raise.EventWith(new BuildProgressEventArgs(_buildable1));

//            Assert.AreEqual(1 - _buildable1.BuildProgress, _fillableImage.FillAmount);
//        }

//        [Test]
//        public void BuildableCompleted_HidesProgress_AndUnsubsribes()
//        {
//            _buildProgress.ShowBuildProgress(_buildable1);
//            _buildProgress.HideBuildProgress();

//            Assert.IsFalse(_fillableImage.IsVisible);
//            AssertAreUnsubribed(_buildable1);
//        }

//        [Test]
//        public void BuidlableDestroyed_HidesProgress_AndUnsubscribes()
//        {
//            _buildProgress.ShowBuildProgress(_buildable1);
//            _buildable1.Destroyed += Raise.EventWith(new DestroyedEventArgs(_buildable1));

//            Assert.IsFalse(_fillableImage.IsVisible);
//            AssertAreUnsubribed(_buildable1);
//        }

//        [Test]
//        public void HideBuildProgress_HidesProgress_AndUnsubsribes()
//        {
//            _buildProgress.ShowBuildProgress(_buildable1);
//            _buildable1.CompletedBuildable += Raise.Event();

//            Assert.IsFalse(_fillableImage.IsVisible);
//            AssertAreUnsubribed(_buildable1);
//        }

//        private void AssertAreUnsubribed(IBuildable buildable)
//        {
//            _fillableImage.ClearReceivedCalls();

//            buildable.BuildableProgress += Raise.EventWith(new BuildProgressEventArgs(buildable));

//            _fillableImage.DidNotReceiveWithAnyArgs().FillAmount = default(float);
//        }
//    }
//}