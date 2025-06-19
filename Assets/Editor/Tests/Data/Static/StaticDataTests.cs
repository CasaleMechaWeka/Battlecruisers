using BattleCruisers.Data;
using BattleCruisers.Data.Models;
using BattleCruisers.Data.Models.PrefabKeys;
using BattleCruisers.Data.Static;
using BattleCruisers.Data.Static.LevelLoot;
using NUnit.Framework;
using System.Collections.Generic;
using UnityAsserts = UnityEngine.Assertions;

namespace BattleCruisers.Tests.Data.Static
{
    public class StaticDataTests
    {
        [Test]
        public void InitialGameModel()
        {
            GameModel initialGameModel = StaticData.InitialGameModel;

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
            Assert.AreEqual(25, StaticData.Levels.Count);
        }

        [Test]
        public void BuildingKeys()
        {
            Assert.AreEqual(21, StaticData.BuildingKeys.Count);
        }

        [Test]
        public void AIBannedUltraKeys()
        {
            Assert.AreEqual(2, StaticData.AIBannedUltrakeys.Count);
        }

        #region GetLevelLoot
        [Test]
        public void GetLevelLoot_TooSmallLevelNum_Throws()
        {
            Assert.Throws<UnityAsserts.AssertionException>(() => StaticData.GetLevelLoot(levelCompleted: 0));
        }

        [Test]
        public void GetLevelLoot_TooLargeLevelNum_Throws()
        {
            Assert.Throws<UnityAsserts.AssertionException>(() => StaticData.GetLevelLoot(levelCompleted: 29));
        }

        [Test]
        public void GetLevelLoot_Unit()
        {
            // Archon
            ILoot actualLoot = StaticData.GetLevelLoot(levelCompleted: 16);
            ILoot expectedLoot = CreateLoot(unitKeys: new UnitKey[] { StaticPrefabKeys.Units.ArchonBattleship });

            Assert.AreEqual(expectedLoot, actualLoot);
        }

        [Test]
        public void GetLevelLoot_Building()
        {
            // Shield
            ILoot actualLoot = StaticData.GetLevelLoot(levelCompleted: 1);
            ILoot expectedLoot = CreateLoot(buildingKeys: new BuildingKey[] { StaticPrefabKeys.Buildings.ShieldGenerator });

            Assert.AreEqual(expectedLoot, actualLoot);
        }

        [Test]
        public void GetLevelLoot_2Buildings()
        {
            // Stealth generator, spy satellite launcher
            ILoot actualLoot = StaticData.GetLevelLoot(levelCompleted: 15);
            ILoot expectedLoot = CreateLoot(buildingKeys: new BuildingKey[] { StaticPrefabKeys.Buildings.StealthGenerator, StaticPrefabKeys.Buildings.SpySatelliteLauncher });

            Assert.AreEqual(expectedLoot, actualLoot);
        }

        [Test]
        public void GetLevelLoot_Hull()
        {
            // Bullshark
            ILoot actualLoot = StaticData.GetLevelLoot(levelCompleted: 3);
            ILoot expectedLoot = CreateLoot(hullKeys: new HullKey[] { StaticPrefabKeys.Hulls.Raptor });

            Assert.AreEqual(expectedLoot, actualLoot);
        }

        [Test]
        public void GetLevelLoot_UnitAndBuilding()
        {
            // Mortar, frigate
            ILoot actualLoot = StaticData.GetLevelLoot(levelCompleted: 2);
            ILoot expectedLoot
                = CreateLoot(
                    unitKeys: new UnitKey[] { StaticPrefabKeys.Units.Frigate },
                    buildingKeys: new BuildingKey[] { StaticPrefabKeys.Buildings.Mortar });

            Assert.AreEqual(expectedLoot, actualLoot);
        }

        [Test]
        public void GetLevelLoot_NoneMoreThan2LootItems()
        {
            foreach (Level level in StaticData.Levels)
            {
                ILoot loot = StaticData.GetLevelLoot(level.Num);
                Assert.IsTrue(loot.Items.Count <= 2);
            }
        }
        #endregion GetLevelLoot

        private ILoot CreateLoot(
            IList<HullKey> hullKeys = null,
            IList<UnitKey> unitKeys = null,
            IList<BuildingKey> buildingKeys = null)
        {
            return
                new Loot(
                    hullKeys ?? new List<HullKey>(),
                    unitKeys ?? new List<UnitKey>(),
                    buildingKeys ?? new List<BuildingKey>());
        }
    }
}
