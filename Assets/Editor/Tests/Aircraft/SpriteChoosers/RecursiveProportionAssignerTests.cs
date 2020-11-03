using BattleCruisers.Buildables.Units.Aircraft.SpriteChoosers;
using NUnit.Framework;
using UnityAsserts = UnityEngine.Assertions;

namespace BattleCruisers.Tests.Aircraft.SpriteChoosers
{
    public class RecursiveProportionAssignerTests
    {
        private IAssigner _assigner;

        private float _cutoff1, _cutoff2;

        [SetUp]
        public void SetuUp()
        {
            int numOfOptions = 3;
            float baseCutoff = 0.5f;

            _assigner = new RecursiveProportionAssigner(numOfOptions, baseCutoff);

            _cutoff1 = baseCutoff;
            _cutoff2 = baseCutoff * baseCutoff;
        }

        [Test]
        public void Max_Option0()
        {
            int assignedOption = _assigner.Assign(1);
            Assert.AreEqual(0, assignedOption);
        }

        [Test]
        public void LargerThanCutoff1_Option0()
        {
            int assignedOption = _assigner.Assign(_cutoff1 + 0.001f);
            Assert.AreEqual(0, assignedOption);
        }

        [Test]
        public void LargerThanCutoff2_Option1()
        {
            int assignedOption = _assigner.Assign(_cutoff2 + 0.001f);
            Assert.AreEqual(1, assignedOption);
        }

        [Test]
        public void SmallerThanCutoff2_Option2()
        {
            int assignedOption = _assigner.Assign(_cutoff2 - 0.001f);
            Assert.AreEqual(2, assignedOption);
        }

        [Test]
        public void Min_Option2()
        {
            int assignedOption = _assigner.Assign(0);
            Assert.AreEqual(2, assignedOption);
        }
    }
}
