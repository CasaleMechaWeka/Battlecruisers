using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using BattleCruisers.Buildables.Buildings;
using BattleCruisers.Buildables.Units;
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
			set { _numOfLevelsCompleted = value; }
		}

		public Loadout PlayerLoadout
		{
			get { return _playerLoadout; }
			set { _playerLoadout = value; }
		}

		public BattleResult LastBattleResult
		{
			get { return _lastBattleResult; }
            set { _lastBattleResult = value; }
		}

        public ReadOnlyCollection<HullKey> UnlockedHulls { get; private set; }
        public ReadOnlyCollection<BuildingKey> UnlockedBuildings { get; private set; }
        public ReadOnlyCollection<UnitKey> UnlockedUnits { get; private set; }

        public GameModel()
		{
			_unlockedHulls = new List<HullKey>();
            UnlockedHulls = _unlockedHulls.AsReadOnly();

			_unlockedBuildings = new List<BuildingKey>();
            UnlockedBuildings = _unlockedBuildings.AsReadOnly();

			_unlockedUnits = new List<UnitKey>();
            UnlockedUnits = _unlockedUnits.AsReadOnly();
		}

		public GameModel(
			int numOfLevelsCompleted,
			Loadout playerLoadout,
			BattleResult lastBattleResult,
			List<HullKey> unlockedHulls,
			List<BuildingKey> unlockedBuildings,
			List<UnitKey> unlockedUnits)
            : this()
		{
			NumOfLevelsCompleted = numOfLevelsCompleted;
			PlayerLoadout = playerLoadout;
			LastBattleResult = lastBattleResult;

            _unlockedHulls.AddRange(unlockedHulls);
            _unlockedBuildings.AddRange(unlockedBuildings);
            _unlockedUnits.AddRange(unlockedUnits);
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

        public IList<UnitKey> GetUnlockedUnits(UnitCategory unitCategory)
        {
            return _unlockedUnits.Where(unitKey => unitKey.UnitCategory == unitCategory).ToList();
        }

		public override bool Equals(object obj)
		{
			GameModel other = obj as GameModel;

			return other != null
				&& other.NumOfLevelsCompleted == NumOfLevelsCompleted
                && PlayerLoadout.SmartEquals(other.PlayerLoadout)
                && LastBattleResult.SmartEquals(other.LastBattleResult)
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
