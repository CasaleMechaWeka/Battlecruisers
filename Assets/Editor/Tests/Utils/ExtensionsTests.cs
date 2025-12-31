using BattleCruisers.Utils;
using NUnit.Framework;

namespace BattleCruisers.Tests.Utils
{
    public class ExtensionsTests
    {
        [Test]
        public void SmartEquals_NullNull()
        {
            string s1 = null;
            string s2 = null;
            Assert.IsTrue(s1.SmartEquals(s2));
        }

        [Test]
        public void SmartEquals_NullNotNull()
        {
            string s1 = null;
            string s2 = "habits";
            Assert.IsFalse(s1.SmartEquals(s2));
        }

        [Test]
        public void SmartEquals_NotNullNull()
        {
            string s1 = "power";
            string s2 = null;
            Assert.IsFalse(s1.SmartEquals(s2));
        }

        [Test]
        public void SmartEquals_NonNull_Different()
        {
            string s1 = "ravine";
            string s2 = "peak";
            Assert.IsFalse(s1.SmartEquals(s2));
        }

        [Test]
        public void SmartEquals_NonNull_Same()
        {
            string s1 = "powder";
            string s2 = "powder";
            Assert.IsTrue(s1.SmartEquals(s2));
        }
    }
}