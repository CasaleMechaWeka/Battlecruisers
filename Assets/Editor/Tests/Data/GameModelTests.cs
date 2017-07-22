using BattleCruisers.Buildables.Buildings;
using BattleCruisers.Buildables.Units;
using BattleCruisers.Data;
using BattleCruisers.Data.PrefabKeys;
using NUnit.Framework;
using UnityAsserts = UnityEngine.Assertions;

namespace BattleCruisers.Tests.Data
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
			UnityAsserts.Assert.raiseExceptions = true;

			_gameModel = new GameModel();

			_hull = new HullKey("Feuer");
			_building = new BuildingKey(BuildingCategory.Factory, "Waser");
			_unit = new UnitKey(UnitCategory.Naval, "Brot");
		}

		#region AddUnlockedHull
		[Test]
		public void AddUnlockedHull()
		{
			Assert.IsFalse(_gameModel.UnlockedHulls.Contains(_hull));
			_gameModel.AddUnlockedHull(_hull);
			Assert.IsTrue(_gameModel.UnlockedHulls.Contains(_hull));
		}

		[Test]
		public void AddUnlockedHull_ExistingHull_Throws()
		{
			AddUnlockedHull();
			Assert.Throws<UnityAsserts.AssertionException>(() => _gameModel.AddUnlockedHull(_hull));
		}
		#endregion AddUnlockedHull


		#region AddUnlockedBuilding
		[Test]
		public void AddUnlockedBuilding()
		{
			Assert.IsFalse(_gameModel.UnlockedBuildings.Contains(_building));
			_gameModel.AddUnlockedBuilding(_building);
			Assert.IsTrue(_gameModel.UnlockedBuildings.Contains(_building));
		}

		[Test]
		public void AddUnlockedBuilding_ExistingBuilding_Throws()
		{
			AddUnlockedBuilding();
			Assert.Throws<UnityAsserts.AssertionException>(() => _gameModel.AddUnlockedBuilding(_building));
		}
		#endregion AddUnlockedBuilding

		#region AddUnlockedUnit
		[Test]
		public void AddUnlockedUnit()
		{
			Assert.IsFalse(_gameModel.UnlockedUnits.Contains(_unit));
			_gameModel.AddUnlockedUnit(_unit);
			Assert.IsTrue(_gameModel.UnlockedUnits.Contains(_unit));
		}

		[Test]
		public void AddUnlockedUnit_ExistingUnit_Throws()
		{
			AddUnlockedUnit();
			Assert.Throws<UnityAsserts.AssertionException>(() => _gameModel.AddUnlockedUnit(_unit));
		}
		#endregion AddUnlockedUnit

		#region LastBattleResult
		[Test]
		public void LastBattleResult_Victory_Updates_NumOfLevelsCompleted()
		{
			int numOfLevelsCompleted = _gameModel.NumOfLevelsCompleted;
			int levelNumJustCompleted = numOfLevelsCompleted + 1;
			BattleResult victoryResult = new BattleResult(levelNumJustCompleted, wasVictory: true);
			_gameModel.LastBattleResult = victoryResult;

			Assert.AreEqual(numOfLevelsCompleted + 1, _gameModel.NumOfLevelsCompleted);
		}

		[Test]
		public void LastBattleResult_Victory_DoesNotUpdate_NumOfLevelsCompeleted_IfNotLatestLevel()
		{
			int numOfLevelsCompleted = _gameModel.NumOfLevelsCompleted;
			int notLatelstLevel = numOfLevelsCompleted;
			BattleResult victoryResult = new BattleResult(notLatelstLevel, wasVictory: true);
			_gameModel.LastBattleResult = victoryResult;

			Assert.AreEqual(numOfLevelsCompleted, _gameModel.NumOfLevelsCompleted);
		}

		[Test]
		public void LastBattleResult_Loss_DoesNotUpdate_NumOfLevelsCompleted()
		{
			int numOfLevelsCompleted = _gameModel.NumOfLevelsCompleted;
			int levelNumJustCompleted = numOfLevelsCompleted + 1;
			BattleResult victoryResult = new BattleResult(levelNumJustCompleted, wasVictory: false);
			_gameModel.LastBattleResult = victoryResult;

			Assert.AreEqual(numOfLevelsCompleted, _gameModel.NumOfLevelsCompleted);
		}

		[Test]
		public void LastBattleResult_LevelCompletedMoreThanOneAhead_Throws()
		{
			int numOfLevelsCompleted = _gameModel.NumOfLevelsCompleted;
			int levelNumJustCompleted = numOfLevelsCompleted + 2;
			BattleResult victoryResult = new BattleResult(levelNumJustCompleted, wasVictory: true);

			Assert.Throws<UnityAsserts.AssertionException>(() => _gameModel.LastBattleResult = victoryResult);
		}
		#endregion LastBattleResult
	}
}
