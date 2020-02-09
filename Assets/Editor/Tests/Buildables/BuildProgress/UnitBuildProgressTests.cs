using NUnit.Framework;
using NSubstitute;
using UnityEngine;
using UnityAsserts = UnityEngine.Assertions;
using BattleCruisers.Buildables.BuildProgress;

namespace BattleCruisers.Tests.Buildables.BuildProgress
{
    public class UnitBuildProgressTests
    {
        private IUnitBuildProgress _unitBuildProgress;
        private string _unitName = "Turtle";
        private IBuildProgressFeedback _buildProgressFeedback;

        [SetUp]
        public void TestSetup()
        {
            _buildProgressFeedback = Substitute.For<IBuildProgressFeedback>();

            _unitBuildProgress = new UnitBuildProgress(_unitName, _buildProgressFeedback);
        }

        [Test]
        public void SetFactory_Null()
        {
        }

        [Test]
        public void SetFactory_NonNull()
        {
        }

        [Test]
        public void SetFactory_Unsubscribes()
        {
        }

        [Test]
        public void _factory_StartedBuildingUnit()
        {
        }

        [Test]
        public void _factory_NewUnitChosen()
        {
        }

        [Test]
        public void _factory_UnitUnderConstructionDestroyed()
        {
        }
    }
}