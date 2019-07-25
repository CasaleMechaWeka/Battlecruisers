using BattleCruisers.Utils.BattleScene.Update;
using NSubstitute;
using NUnit.Framework;
using UnityCommon.PlatformAbstractions;

namespace BattleCruisers.Tests.Utils.BattleScene.Update
{
    public class MultiFrameUpdaterTests
    {
        private IUpdater _multiFrameUpdater, _perFrameUpdater;
        private IDeltaTimeProvider _timeProvider;
        private float _intervalInS = 0.2f;
        private int _updatedCount;

        [SetUp]
        public void TestSetup()
        {
            _perFrameUpdater = Substitute.For<IUpdater>();
            _timeProvider = Substitute.For<IDeltaTimeProvider>();

            _multiFrameUpdater = new MultiFrameUpdater(_perFrameUpdater, _timeProvider, _intervalInS);

            _updatedCount = 0;
            _multiFrameUpdater.Updated += (sender, e) => _updatedCount++;

            _timeProvider.DeltaTime.Returns(0.1f);
        }

        [Test]
        public void Updated_NotEnoughTimeHasPassed_DoesNothing()
        {
            _perFrameUpdater.Updated += Raise.Event();
            Assert.AreEqual(0, _updatedCount);
        }

        [Test]
        public void Updated_EnoughTimeHasPassed_EmitsEvent()
        {
            _perFrameUpdater.Updated += Raise.Event();
            _perFrameUpdater.Updated += Raise.Event();
            Assert.AreEqual(1, _updatedCount);
        }

        [Test]
        public void Updated_ResetsTimeWaited()
        {
            _perFrameUpdater.Updated += Raise.Event();
            _perFrameUpdater.Updated += Raise.Event();
            Assert.AreEqual(1, _updatedCount);

            // Internal count is reset, another event is not raised
            _perFrameUpdater.Updated += Raise.Event();
            Assert.AreEqual(1, _updatedCount);
        }
    }
}