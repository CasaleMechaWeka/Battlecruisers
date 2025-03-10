using BattleCruisers.UI;
using BattleCruisers.Utils.BattleScene.Update;
using BattleCruisers.Utils.PlatformAbstractions.Time;
using NSubstitute;
using NUnit.Framework;

namespace BattleCruisers.Tests.UI
{
    public class LongPressIdentifierTests
    {
        private ILongPressIdentifier _longPressIdentifier;

        private IPointerUpDownEmitter _button;
        private ITime _time;
        private IUpdater _updater;
        private float _intervalLengthS;
        private int _startCount, _endCount, _intervalCount;

        [SetUp]
        public void TestSetup()
        {
            _button = Substitute.For<IPointerUpDownEmitter>();
            _time = Substitute.For<ITime>();
            _updater = Substitute.For<IUpdater>();
            _intervalLengthS = 0.25f;

            _longPressIdentifier
                = new LongPressIdentifier(
                    _button,
                    _time,
                    _updater,
                    _intervalLengthS);

            _startCount = 0;
            _longPressIdentifier.LongPressStart += (sender, e) => _startCount++;

            _endCount = 0;
            _longPressIdentifier.LongPressEnd += (sender, e) => _endCount++;

            _intervalCount = 0;
            _longPressIdentifier.LongPressInterval += (sender, e) => _intervalCount++;
        }

        [Test]
        public void InitialState()
        {
            Assert.AreEqual(0, _longPressIdentifier.IntervalNumber);
        }

        [Test]
        public void _button_PointerDown()
        {
            _button.PointerDown += Raise.Event();
            Assert.AreEqual(1, _startCount);
        }

        [Test]
        public void _button_PointerUp()
        {
            _button.PointerUp += Raise.Event();
            Assert.AreEqual(1, _endCount);
        }

        [Test]
        public void _updater_Updated_IgnoredOutsideOfLongPress()
        {
            _updater.Updated += Raise.Event();
            var compilerBribe = _time.DidNotReceive().UnscaledDeltaTime;
        }

        [Test]
        public void _updater_Updated_IntervalNotReached()
        {
            _button.PointerDown += Raise.Event();
            _time.UnscaledDeltaTime.Returns(_intervalLengthS - 0.1f);

            _updater.Updated += Raise.Event();

            Assert.AreEqual(0, _intervalCount);
            Assert.AreEqual(0, _longPressIdentifier.IntervalNumber);
        }

        [Test]
        public void _updater_Updated_IntervalReached()
        {
            _button.PointerDown += Raise.Event();
            _time.UnscaledDeltaTime.Returns(_intervalLengthS - 0.1f);

            _updater.Updated += Raise.Event(); // Not past interval
            _updater.Updated += Raise.Event(); // Past interval

            Assert.AreEqual(1, _intervalCount);
            Assert.AreEqual(1, _longPressIdentifier.IntervalNumber);
        }

        [Test]
        public void TwoConsecutiveLongPresses()
        {
            // First long press, 3 intervals
            _button.PointerDown += Raise.Event();
            _time.UnscaledDeltaTime.Returns(_intervalLengthS - 0.1f);

            _updater.Updated += Raise.Event(); // 0.15
            _updater.Updated += Raise.Event(); // 0.3 / 0.25 => 1st interval
            _updater.Updated += Raise.Event(); // 0.45
            _updater.Updated += Raise.Event(); // 0.6 / 0.25 => 2nd interval
            _updater.Updated += Raise.Event(); // 0.75 / 0.25 => 3rd interval

            Assert.AreEqual(3, _longPressIdentifier.IntervalNumber);
            Assert.AreEqual(3, _intervalCount);

            _button.PointerUp += Raise.Event();

            // Update ignored
            _intervalCount = 0;

            _updater.Updated += Raise.Event(); // 0.15
            _updater.Updated += Raise.Event(); // 0.3 / 0.25 => 1st interval, but updates are ignored

            Assert.AreEqual(0, _intervalCount);

            // Second long press, 5 intervals
            _button.PointerDown += Raise.Event();
            Assert.AreEqual(0, _longPressIdentifier.IntervalNumber);
            _time.UnscaledDeltaTime.Returns(_intervalLengthS - 0.1f);

            _updater.Updated += Raise.Event(); // 0.15
            _updater.Updated += Raise.Event(); // 0.3 / 0.25 => 1st interval
            _updater.Updated += Raise.Event(); // 0.45
            _updater.Updated += Raise.Event(); // 0.6 / 0.25 => 2nd interval
            _updater.Updated += Raise.Event(); // 0.75 / 0.25 => 3rd interval
            _updater.Updated += Raise.Event(); // 0.9
            _updater.Updated += Raise.Event(); // 1.05 / 0.25 => 4th interval
            _updater.Updated += Raise.Event(); // 1.2
            _updater.Updated += Raise.Event(); // 1.35 / 0.25 => 5th interval

            Assert.AreEqual(5, _longPressIdentifier.IntervalNumber);
            Assert.AreEqual(5, _intervalCount);

            _button.PointerUp += Raise.Event();
        }
    }
}