using BattleCruisers.Data.Models;
using BattleCruisers.Data.Static;
using NUnit.Framework;
using UnityAsserts = UnityEngine.Assertions;

namespace BattleCruisers.Tests.Data.Static
{
    public class StaticDataTests
    {
        private IStaticData _staticData;

        [SetUp]
        public void SetuUp()
        {
            UnityAsserts.Assert.raiseExceptions = true;

            _staticData = new StaticData();
        }

        [Test]
        public void InitialGameModel()
        {
            IGameModel initialGameModel = _staticData.InitialGameModel;

            Assert.AreEqual(0, initialGameModel.NumOfLevelsCompleted);
            Assert.AreEqual(StaticPrefabKeys.Hulls.Trident, initialGameModel.PlayerLoadout.Hull);
            Assert.IsNull(initialGameModel.LastBattleResult);

            Assert.AreEqual(1, initialGameModel.UnlockedHulls.Count);
            Assert.AreEqual(6, initialGameModel.UnlockedBuildings.Count);
            Assert.AreEqual(2, initialGameModel.UnlockedUnits.Count);
        }

        [Test]
        public void Levels()
        {
            Assert.AreEqual(21, _staticData.Levels.Count);
        }

        [Test]
        public void BuildingKeys()
        {
            Assert.AreEqual(21, _staticData.BuildingKeys.Count);
        }

        [Test]
        public void AIBannedUltraKeys()
        {
            Assert.AreEqual(2, _staticData.AIBannedUltrakeys.Count);
        }

        [Test]
        public void IsUnitAvailable()
        {
            // FELIX  NEXT
        }
    }
}
