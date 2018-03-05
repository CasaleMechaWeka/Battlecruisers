using BattleCruisers.Buildables.Buildings;
using BattleCruisers.Buildables.Units;
using BattleCruisers.Data.Models;
using BattleCruisers.Data.Models.PrefabKeys;
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
			_building = new BuildingKey(BuildingCategory.Factory, "Wasser");
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
	}
}
