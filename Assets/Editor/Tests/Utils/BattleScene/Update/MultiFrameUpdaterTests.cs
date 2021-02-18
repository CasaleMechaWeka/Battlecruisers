using BattleCruisers.Utils.BattleScene.Update;
using NSubstitute;
using NUnit.Framework;
using BattleCruisers.Utils.PlatformAbstractions.Time;

namespace BattleCruisers.Tests.Utils.BattleScene.Update
{
    public class MultiFrameUpdaterTests
    {
        private IUpdater _multiFrameUpdater, _perFrameUpdater;
        private IDeltaTimeProvider _timeProvider;
        private float _intervalInS = 0.3f;
        private int _updatedCount;
        private const float DELTA_TIME_INCREMENT = 0.1f;

        [SetUp]
        public void TestSetup()
        {
            _perFrameUpdater = Substitute.For<IUpdater>();
            _timeProvider = Substitute.For<IDeltaTimeProvider>();

            _multiFrameUpdater = new MultiFrameUpdater(_perFrameUpdater, _timeProvider, _intervalInS);

            _updatedCount = 0;
            _multiFrameUpdater.Updated += (sender, e) => _updatedCount++;

            _timeProvider.DeltaTime.Returns(DELTA_TIME_INCREMENT);
        }

        [Test]
        public void InitialState()
        {
            Assert.AreEqual(0, _multiFrameUpdater.DeltaTime);
        }

        [Test]
        public void Updated_NotEnoughTimeHasPassed_DoesNothing()
        {
            _perFrameUpdater.Updated += Raise.Event();

            Assert.AreEqual(DELTA_TIME_INCREMENT, _multiFrameUpdater.DeltaTime);
            Assert.AreEqual(0, _updatedCount);
        }

        [Test]
        public void Updated_EnoughTimeHasPassed_EmitsEvent()
        {
            _perFrameUpdater.Updated += Raise.Event();
            Assert.AreEqual(DELTA_TIME_INCREMENT, _multiFrameUpdater.DeltaTime);

            _perFrameUpdater.Updated += Raise.Event();
            Assert.AreEqual(2 * DELTA_TIME_INCREMENT, _multiFrameUpdater.DeltaTime);

            _perFrameUpdater.Updated += Raise.Event();
            Assert.AreEqual(0, _multiFrameUpdater.DeltaTime);
            Assert.AreEqual(1, _updatedCount);
        }

        [Test]
        public void Updated_ResetsTimeWaited()
        {
            _perFrameUpdater.Updated += Raise.Event();
            _perFrameUpdater.Updated += Raise.Event();
            _perFrameUpdater.Updated += Raise.Event();

            Assert.AreEqual(1, _updatedCount);
            Assert.AreEqual(0, _multiFrameUpdater.DeltaTime);

            // Internal count is reset, another event is not raised
            _perFrameUpdater.Updated += Raise.Event();
            Assert.AreEqual(1, _updatedCount);
            Assert.AreEqual(DELTA_TIME_INCREMENT, _multiFrameUpdater.DeltaTime);
        }
    }
}