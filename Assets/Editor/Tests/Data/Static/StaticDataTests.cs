using System.Collections.Generic;
using BattleCruisers.Buildables.Buildings;
using BattleCruisers.Buildables.Units;
using BattleCruisers.Data;
using BattleCruisers.Data.Models;
using BattleCruisers.Data.Models.PrefabKeys;
using BattleCruisers.Data.Static;
using BattleCruisers.Data.Static.LevelLoot;
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
            Assert.AreEqual(28, _staticData.Levels.Count);
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

        #region IsUnitAvailable
        [Test]
        public void IsUnitAvailable_NonExistantUnitKey_Throws()
        {
            UnitKey bogusKey = new UnitKey(UnitCategory.Aircraft, "Breckenridge");
            Assert.Throws<UnityAsserts.AssertionException>(() => _staticData.IsUnitAvailable(bogusKey, levelNum: 1));
        }

        [Test]
        public void IsUnitAvailable_True()
        {
            Assert.IsTrue(_staticData.IsUnitAvailable(StaticPrefabKeys.Units.AttackBoat, levelNum: 1));
        }

        [Test]
        public void IsUnitAvailable_False()
        {
            Assert.IsFalse(_staticData.IsUnitAvailable(StaticPrefabKeys.Units.Frigate, levelNum: 1));
        }
        #endregion IsUnitAvailable

        #region IsBuildingAvailable
        [Test]
        public void IsBuildingAvailable_NonExistantBuildingKey_Throws()
        {
            BuildingKey bogusKey = new BuildingKey(BuildingCategory.Ultra, "Veil");
            Assert.Throws<UnityAsserts.AssertionException>(() => _staticData.IsBuildingAvailable(bogusKey, levelNum: 1));
        }

        [Test]
        public void IsBuildingAvailable_True()
        {
            Assert.IsTrue(_staticData.IsBuildingAvailable(StaticPrefabKeys.Buildings.AirFactory, levelNum: 1));
        }

        [Test]
        public void IsBuildingAvailable_False()
        {
            Assert.IsFalse(_staticData.IsBuildingAvailable(StaticPrefabKeys.Buildings.Railgun, levelNum: 1));
        }
        #endregion IsBuildingAvailable

        [Test]
        public void GetAvailableUnits()
        {
            Assert.AreEqual(1, _staticData.GetAvailableUnits(UnitCategory.Aircraft, levelNum: 1).Count);
            Assert.AreEqual(1, _staticData.GetAvailableUnits(UnitCategory.Naval, levelNum: 1).Count);
            Assert.AreEqual(0, _staticData.GetAvailableUnits(UnitCategory.Untouchable, levelNum: 1).Count);
        }

        [Test]
        public void GetAvailableBuildings()
        {
            Assert.AreEqual(2, _staticData.GetAvailableBuildings(BuildingCategory.Defence, levelNum: 1).Count);
            Assert.AreEqual(3, _staticData.GetAvailableBuildings(BuildingCategory.Factory, levelNum: 1).Count);
            Assert.AreEqual(1, _staticData.GetAvailableBuildings(BuildingCategory.Offence, levelNum: 1).Count);
            Assert.AreEqual(0, _staticData.GetAvailableBuildings(BuildingCategory.Tactical, levelNum: 1).Count);
            Assert.AreEqual(0, _staticData.GetAvailableBuildings(BuildingCategory.Ultra, levelNum: 1).Count);
        }

        #region GetLevelLoot
        [Test]
        public void GetLevelLoot_TooSmallLevelNum_Throws()
        {
            Assert.Throws<UnityAsserts.AssertionException>(() => _staticData.GetLevelLoot(levelCompleted: 0));
        }

        [Test]
        public void GetLevelLoot_TooLargeLevelNum_Throws()
        {
            Assert.Throws<UnityAsserts.AssertionException>(() => _staticData.GetLevelLoot(levelCompleted: 29));
        }

        [Test]
        public void GetLevelLoot_Unit()
        {
            // Archon
            ILoot actualLoot = _staticData.GetLevelLoot(levelCompleted: 19);
            ILoot expectedLoot = CreateLoot(unitKeys: new UnitKey[] { StaticPrefabKeys.Units.ArchonBattleship });

            Assert.AreEqual(expectedLoot, actualLoot);
        }

        [Test]
        public void GetLevelLoot_2Units()
        {
            // Fighter, destroyer
            ILoot actualLoot = _staticData.GetLevelLoot(levelCompleted: 8);
            ILoot expectedLoot = CreateLoot(unitKeys: new UnitKey[] { StaticPrefabKeys.Units.Fighter, StaticPrefabKeys.Units.Destroyer});

            Assert.AreEqual(expectedLoot, actualLoot);
        }

        [Test]
        public void GetLevelLoot_Building()
        {
            // Shield
            ILoot actualLoot = _staticData.GetLevelLoot(levelCompleted: 1);
            ILoot expectedLoot = CreateLoot(buildingKeys: new BuildingKey[] { StaticPrefabKeys.Buildings.ShieldGenerator});

            Assert.AreEqual(expectedLoot, actualLoot);
        }

        [Test]
        public void GetLevelLoot_2Buildings()
        {
            // Stealth generator, spy satellite launcher
            ILoot actualLoot = _staticData.GetLevelLoot(levelCompleted: 13);
            ILoot expectedLoot = CreateLoot(buildingKeys: new BuildingKey[] { StaticPrefabKeys.Buildings.StealthGenerator, StaticPrefabKeys.Buildings.SpySatelliteLauncher});

            Assert.AreEqual(expectedLoot, actualLoot);
        }

        [Test]
        public void GetLevelLoot_Hull()
        {
            // Bullshark
            ILoot actualLoot = _staticData.GetLevelLoot(levelCompleted: 3);
            ILoot expectedLoot = CreateLoot(hullKeys: new HullKey[] { StaticPrefabKeys.Hulls.Raptor});

            Assert.AreEqual(expectedLoot, actualLoot);
        }

        [Test]
        public void GetLevelLoot_UnitAndBuilding()
        {
            // Mortar, frigate
            ILoot actualLoot = _staticData.GetLevelLoot(levelCompleted: 2);
            ILoot expectedLoot 
                = CreateLoot(
                    unitKeys: new UnitKey[] { StaticPrefabKeys.Units.Frigate },
                    buildingKeys: new BuildingKey[] { StaticPrefabKeys.Buildings.Mortar});

            Assert.AreEqual(expectedLoot, actualLoot);
        }

        [Test]
        public void GetLevelLoot_BuildingAndHull()
        {
            // Rockjaw, deathstar
            ILoot actualLoot = _staticData.GetLevelLoot(levelCompleted: 7);
            ILoot expectedLoot
                = CreateLoot(
                    hullKeys: new HullKey[] { StaticPrefabKeys.Hulls.Rockjaw },
                    buildingKeys: new BuildingKey[] { StaticPrefabKeys.Buildings.DeathstarLauncher });

            Assert.AreEqual(expectedLoot, actualLoot);
        }

        [Test]
        public void GetLevelLoot_NoneMoreThan2LootItems()
        {
            foreach (ILevel level in _staticData.Levels)
            {
                ILoot loot = _staticData.GetLevelLoot(level.Num);
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
