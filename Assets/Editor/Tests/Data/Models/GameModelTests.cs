using BattleCruisers.Buildables.Buildings;
using BattleCruisers.Buildables.Units;
using BattleCruisers.Data.Models;
using BattleCruisers.Data.Models.PrefabKeys;
using BattleCruisers.Data.Settings;
using BattleCruisers.Data.Static;
using NUnit.Framework;
using System.Collections.Generic;
using System.Linq;
using UnityAsserts = UnityEngine.Assertions;

namespace BattleCruisers.Tests.Data.Models
{
    public class GameModelTests
	{
		private IGameModel _gameModel;

		private HullKey _hull;
		private BuildingKey _building;
		private UnitKey _unit;

		[SetUp]
		public void SetuUp()
		{
			_gameModel = new GameModel();

			_hull = new HullKey("Feuer");
			_building = new BuildingKey(BuildingCategory.Factory, "Wasser");
			_unit = new UnitKey(UnitCategory.Naval, "Brot");
		}

		#region AddUnlockedHull
		[Test]
		public void AddUnlockedHull()
		{
			Assert.IsFalse(_gameModel.UnlockedHulls.Contains(_hull));
			Assert.IsFalse(_gameModel.NewHulls.Items.Contains(_hull));

			_gameModel.AddUnlockedHull(_hull);
			Assert.IsTrue(_gameModel.UnlockedHulls.Contains(_hull));
			Assert.IsTrue(_gameModel.NewHulls.Items.Contains(_hull));
		}

		[Test]
		public void AddUnlockedHull_ExistingHull_DoesNoting()
		{
			_gameModel.AddUnlockedHull(_hull);
			Assert.AreEqual(1, FindCount(_gameModel.UnlockedHulls, _hull));

			_gameModel.AddUnlockedHull(_hull);
			Assert.AreEqual(1, FindCount(_gameModel.UnlockedHulls, _hull));
		}
		#endregion AddUnlockedHull

		#region AddUnlockedBuilding
		[Test]
		public void AddUnlockedBuilding()
		{
			Assert.IsFalse(_gameModel.UnlockedBuildings.Contains(_building));
			Assert.IsFalse(_gameModel.NewBuildings.Items.Contains(_building));

            _gameModel.AddUnlockedBuilding(_building);

			Assert.IsTrue(_gameModel.UnlockedBuildings.Contains(_building));
			Assert.IsTrue(_gameModel.NewBuildings.Items.Contains(_building));
		}

		[Test]
		public void AddUnlockedBuilding_ExistingBuilding_DoesNothing()
		{
			_gameModel.AddUnlockedBuilding(_building);
			Assert.AreEqual(1, FindCount(_gameModel.UnlockedBuildings, _building));

			_gameModel.AddUnlockedBuilding(_building);
			Assert.AreEqual(1, FindCount(_gameModel.UnlockedBuildings, _building));
		}
		#endregion AddUnlockedBuilding

		#region AddUnlockedUnit
		[Test]
		public void AddUnlockedUnit()
		{
			Assert.IsFalse(_gameModel.UnlockedUnits.Contains(_unit));
			Assert.IsFalse(_gameModel.NewUnits.Items.Contains(_unit));

            _gameModel.AddUnlockedUnit(_unit);
            
			Assert.IsTrue(_gameModel.UnlockedUnits.Contains(_unit));
			Assert.IsTrue(_gameModel.NewUnits.Items.Contains(_unit));
		}

		[Test]
		public void AddUnlockedUnit_ExistingUnit_DoesNothing()
		{
			_gameModel.AddUnlockedUnit(_unit);
			Assert.AreEqual(1, FindCount(_gameModel.UnlockedUnits, _unit));

			_gameModel.AddUnlockedUnit(_unit);
			Assert.AreEqual(1, FindCount(_gameModel.UnlockedUnits, _unit));
		}
        #endregion AddUnlockedUnit

        #region AddCompletedLevel
        [Test]
        public void AddCompletedLevel_HaveNotCompletedPreceedingLevel_Throws()
        {
            CompletedLevel level = new CompletedLevel(levelNum: 2, hardestDifficulty: Difficulty.Easy);
            Assert.Throws<UnityAsserts.AssertionException>(() => _gameModel.AddCompletedLevel(level));
        }

        [Test]
        public void AddCompletedLevel_TooSmallLevelNum_Throws()
        {
            CompletedLevel level = new CompletedLevel(levelNum: 0, hardestDifficulty: Difficulty.Easy);
            Assert.Throws<UnityAsserts.AssertionException>(() => _gameModel.AddCompletedLevel(level));
        }

        [Test]
        public void AddCompletedLevel_FirstTimeLevelCompleted_AddsLevel()
        {
            CompletedLevel level = new CompletedLevel(levelNum: 1, hardestDifficulty: Difficulty.Easy);
            _gameModel.AddCompletedLevel(level);
            Assert.AreEqual(1, _gameModel.CompletedLevels.Count);
            Assert.AreSame(level, _gameModel.CompletedLevels[0]);
        }

        [Test]
        public void AddCompletedLevel_SecondTimeLevelCompleted_HarderDifficulty_UpdatesDifficulty()
        {
            // Complete first time on easy
            CompletedLevel firstTime = new CompletedLevel(levelNum: 1, hardestDifficulty: Difficulty.Easy);
            _gameModel.AddCompletedLevel(firstTime);

            // Complete second time on normal
            CompletedLevel secondTime = new CompletedLevel(levelNum: 1, hardestDifficulty: Difficulty.Normal);
            _gameModel.AddCompletedLevel(secondTime);
            Assert.AreEqual(1, _gameModel.CompletedLevels.Count);
            Assert.AreEqual(secondTime, _gameModel.CompletedLevels[0]);
        }

        [Test]
        public void AddCompletedLevel_SecondTimeLevelCompleted_EasierDifficulty_DoesNotUpdateDifficulty()
        {
            // Complete first time on normal
            CompletedLevel firstTime = new CompletedLevel(levelNum: 1, hardestDifficulty: Difficulty.Normal);
            _gameModel.AddCompletedLevel(firstTime);

            // Complete second time on easy
            CompletedLevel secondTime = new CompletedLevel(levelNum: 1, hardestDifficulty: Difficulty.Easy);
            _gameModel.AddCompletedLevel(secondTime);
            Assert.AreEqual(1, _gameModel.CompletedLevels.Count);
            Assert.AreSame(firstTime, _gameModel.CompletedLevels[0]);
        }
        #endregion AddCompletedLevel

		[Test]
		public void IsUnitUnlocked_False()
        {
			Assert.IsFalse(_gameModel.IsUnitUnlocked(StaticPrefabKeys.Units.ArchonBattleship));
		}

		[Test]
		public void IsUnitUnlocked_True()
		{
			UnitKey unitKey = StaticPrefabKeys.Units.ArchonBattleship;
			_gameModel.AddUnlockedUnit(unitKey);
            Assert.IsTrue(_gameModel.IsUnitUnlocked(unitKey));
		}

		[Test]
		public void IsBuildingUnlocked_False()
		{
			Assert.IsFalse(_gameModel.IsBuildingUnlocked(StaticPrefabKeys.Buildings.DeathstarLauncher));
		}

		[Test]
		public void IsBuildingUnlocked_True()
		{
			BuildingKey buildingKey = StaticPrefabKeys.Buildings.DeathstarLauncher;
			_gameModel.AddUnlockedBuilding(buildingKey);
			Assert.IsTrue(_gameModel.IsBuildingUnlocked(buildingKey));
		}

		[Test]
		public void LastBattleResult_ResetsSelectedLevel()
        {
			_gameModel.SelectedLevel = 3;
			_gameModel.LastBattleResult = null;

			Assert.AreEqual(GameModel.UNSET_SELECTED_LEVEL, _gameModel.SelectedLevel);
        }

		private int FindCount<TKey>(ICollection<TKey> list, TKey instance)
        {
			return list.Count(item => ReferenceEquals(item, instance));
		}
	}
}
