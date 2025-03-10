using BattleCruisers.UI.Cameras.Helpers;
using NUnit.Framework;
using UnityEngine;

namespace BattleCruisers.Tests.UI.Cameras.Helpers
{
    public class ScrollRecogniserTests
    {
        private IScrollRecogniser _recogniser;

        [SetUp]
        public void TestSetup()
        {
            _recogniser = new ScrollRecogniser();
        }

        [Test]
        public void Scroll()
        {
            Vector2 delta = new Vector2(2.0001f, 1);
            Assert.IsTrue(_recogniser.IsScroll(delta));
        }

        [Test]
        public void Scroll_DespiteNegative()
        {
            Vector2 delta = new Vector2(-2.0001f, 1);
            Assert.IsTrue(_recogniser.IsScroll(delta));
        }

        [Test]
        public void Zoom()
        {
            Vector2 delta = new Vector2(2, 1);
            Assert.IsTrue(_recogniser.IsScroll(delta));
        }

        [Test]
        public void Zoom_DespiteNegative()
        {
            Vector2 delta = new Vector2(2, -1);
            Assert.IsTrue(_recogniser.IsScroll(delta));
        }
    }
}