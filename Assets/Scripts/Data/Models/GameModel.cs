using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using BattleCruisers.Buildables.Buildings;
using BattleCruisers.Data.Models.PrefabKeys;
using BattleCruisers.Utils;
using UnityEngine;
using UnityEngine.Assertions;

namespace BattleCruisers.Data.Models
{
	[Serializable]
	public class GameModel : IGameModel
	{
		[SerializeField]
		private int _numOfLevelsCompleted;

		[SerializeField]
		private Loadout _playerLoadout;

		[SerializeField]
		private BattleResult _lastBattleResult;

		[SerializeField]
		private List<HullKey> _unlockedHulls;

		[SerializeField]
		private List<BuildingKey> _unlockedBuildings;

		[SerializeField]
		private List<UnitKey> _unlockedUnits;

		public int NumOfLevelsCompleted 
		{ 
			get { return _numOfLevelsCompleted; }
			private set { _numOfLevelsCompleted = value; }
		}

		public Loadout PlayerLoadout
		{
			get { return _playerLoadout; }
			set { _playerLoadout = value; }
		}

		public BattleResult LastBattleResult
		{
			get { return _lastBattleResult; }
			set 
			{ 
				_lastBattleResult = value; 

				if (_lastBattleResult != null &&
					_lastBattleResult.WasVictory && 
					_lastBattleResult.LevelNum > NumOfLevelsCompleted)
				{
					Assert.AreEqual(_lastBattleResult.LevelNum - 1, NumOfLevelsCompleted);
					NumOfLevelsCompleted = _lastBattleResult.LevelNum;
				}
			}
		}

		public ReadOnlyCollection<HullKey> UnlockedHulls { get { return _unlockedHulls.AsReadOnly(); } }
		public ReadOnlyCollection<BuildingKey> UnlockedBuildings { get { return _unlockedBuildings.AsReadOnly(); } }
		public ReadOnlyCollection<UnitKey> UnlockedUnits { get { return _unlockedUnits.AsReadOnly(); } }

		public GameModel()
		{
			_unlockedHulls = new List<HullKey>();
			_unlockedBuildings = new List<BuildingKey>();
			_unlockedUnits = new List<UnitKey>();
		}

		public GameModel(
			int numOfLevelsCompleted,
			Loadout playerLoadout,
			BattleResult lastBattleResult,
			List<HullKey> unlockedHulls,
			List<BuildingKey> unlockedBuildings,
			List<UnitKey> unlockedUnits)
		{
			NumOfLevelsCompleted = numOfLevelsCompleted;
			PlayerLoadout = playerLoadout;
			LastBattleResult = lastBattleResult;
			_unlockedHulls = unlockedHulls;
			_unlockedBuildings = unlockedBuildings;
			_unlockedUnits = unlockedUnits;
		}

		public void AddUnlockedHull(HullKey hull)
		{
			Assert.IsFalse(_unlockedHulls.Contains(hull));
			_unlockedHulls.Add(hull);
		}

		public void AddUnlockedBuilding(BuildingKey building)
		{
			Assert.IsFalse(_unlockedBuildings.Contains(building));
			_unlockedBuildings.Add(building);
		}

		public void AddUnlockedUnit(UnitKey unit)
		{
			Assert.IsFalse(_unlockedUnits.Contains(unit));
			_unlockedUnits.Add(unit);
		}

		public IList<BuildingKey> GetUnlockedBuildings(BuildingCategory buildingCategory)
		{
			return _unlockedBuildings.Where(buildingKey => buildingKey.BuildingCategory == buildingCategory).ToList();
		}

		public override bool Equals(object obj)
		{
			GameModel other = obj as GameModel;

			return other != null
				&& other.NumOfLevelsCompleted == NumOfLevelsCompleted
				&& object.ReferenceEquals(PlayerLoadout, other.PlayerLoadout)
					|| (PlayerLoadout != null && PlayerLoadout.Equals(other.PlayerLoadout))
				&& object.ReferenceEquals(LastBattleResult, other.LastBattleResult)
					|| (LastBattleResult != null && LastBattleResult.Equals(other.LastBattleResult))
				&& Enumerable.SequenceEqual(UnlockedHulls, other.UnlockedHulls)
				&& Enumerable.SequenceEqual(UnlockedBuildings, other.UnlockedBuildings)
				&& Enumerable.SequenceEqual(UnlockedUnits, other.UnlockedUnits);
		}

		public override int GetHashCode()
		{
			return this.GetHashCode(NumOfLevelsCompleted, PlayerLoadout, LastBattleResult, _unlockedHulls, _unlockedUnits, _unlockedBuildings);
		}
	}
}
