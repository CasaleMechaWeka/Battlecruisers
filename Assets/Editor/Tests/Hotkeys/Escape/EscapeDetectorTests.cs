using BattleCruisers.Hotkeys.Escape;
using BattleCruisers.Utils.BattleScene.Update;
using BattleCruisers.Utils.PlatformAbstractions;
using NSubstitute;
using NUnit.Framework;
using UnityEngine;

namespace BattleCruisers.Tests.Hotkeys.Escape
{
    public class EscapeDetectorTests
    {
        private IEscapeDetector _escapeDetector;
        private IInput _input;
        private IUpdater _updater;
        private int _escapeCount;

        [SetUp]
        public void TestSetup()
        {
            _input = Substitute.For<IInput>();
            _updater = Substitute.For<IUpdater>();

            _escapeDetector = new EscapeDetector(_input, _updater);

            _escapeCount = 0;
            _escapeDetector.EscapePressed += (sender, e) => _escapeCount++;
        }

        [Test]
        public void EscapeKey()
        {
            _input.GetKeyUp(KeyCode.Escape).Returns(true);
            _updater.Updated += Raise.Event();
            Assert.AreEqual(1, _escapeCount);
        }

        [Test]
        public void NonEscapeKey()
        {
            _input.GetKeyUp(KeyCode.Escape).Returns(false);
            _updater.Updated += Raise.Event();
            Assert.AreEqual(0, _escapeCount);
        }

        [Test]
        public void Dispose()
        {
            _escapeDetector.DisposeManagedState();
            _input.GetKeyUp(KeyCode.Escape).Returns(true);

            _updater.Updated += Raise.Event();
            
            Assert.AreEqual(0, _escapeCount);
        }
    }
}