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
        private bool _hasAttemptedTutorial;

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

        [SerializeField]
        private List<CompletedLevel> _completedLevels;

        // FELIX  TEMP
        [SerializeField]
        public NewItems<HullKey> newHulls;

		public int NumOfLevelsCompleted => _completedLevels.Count;

        public bool HasAttemptedTutorial
        {
            get { return _hasAttemptedTutorial; }
            set { _hasAttemptedTutorial = value; }
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

        public ReadOnlyCollection<HullKey> UnlockedHulls { get; }
        public ReadOnlyCollection<BuildingKey> UnlockedBuildings { get; }
        public ReadOnlyCollection<UnitKey> UnlockedUnits { get; }
        public ReadOnlyCollection<CompletedLevel> CompletedLevels { get; }

        public GameModel()
		{
			_unlockedHulls = new List<HullKey>();
            UnlockedHulls = _unlockedHulls.AsReadOnly();

			_unlockedBuildings = new List<BuildingKey>();
            UnlockedBuildings = _unlockedBuildings.AsReadOnly();

			_unlockedUnits = new List<UnitKey>();
            UnlockedUnits = _unlockedUnits.AsReadOnly();

            _completedLevels = new List<CompletedLevel>();
            CompletedLevels = _completedLevels.AsReadOnly();

            newHulls = new NewItems<HullKey>();
		}

		public GameModel(
            bool hasAttemptedTutorial,
			Loadout playerLoadout,
			BattleResult lastBattleResult,
			List<HullKey> unlockedHulls,
			List<BuildingKey> unlockedBuildings,
			List<UnitKey> unlockedUnits)
            : this()
		{
            HasAttemptedTutorial = hasAttemptedTutorial;
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

        public void AddCompletedLevel(CompletedLevel completedLevel)
        {
            Assert.IsTrue(completedLevel.LevelNum <= _completedLevels.Count + 1, "Have not completed preceeding level :/");
            Assert.IsTrue(completedLevel.LevelNum > 0);

            if (completedLevel.LevelNum > _completedLevels.Count)
            {
                // First time level has been completed
                _completedLevels.Add(completedLevel);
            }
            else
            {
                // Level has been completed before
                CompletedLevel currentLevel = _completedLevels[completedLevel.LevelNum - 1];

                if (completedLevel.HardestDifficulty > currentLevel.HardestDifficulty)
                {
                    currentLevel.HardestDifficulty = completedLevel.HardestDifficulty;
                }
            }
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
                && other.HasAttemptedTutorial == HasAttemptedTutorial
                && other.NumOfLevelsCompleted == NumOfLevelsCompleted
                && PlayerLoadout.SmartEquals(other.PlayerLoadout)
                && LastBattleResult.SmartEquals(other.LastBattleResult)
                && newHulls.SmartEquals(other.newHulls)
                && Enumerable.SequenceEqual(UnlockedHulls, other.UnlockedHulls)
                && Enumerable.SequenceEqual(UnlockedBuildings, other.UnlockedBuildings)
                && Enumerable.SequenceEqual(UnlockedUnits, other.UnlockedUnits)
                && Enumerable.SequenceEqual(CompletedLevels, other.CompletedLevels);
		}

		public override int GetHashCode()
		{
            return this.GetHashCode(HasAttemptedTutorial, NumOfLevelsCompleted, PlayerLoadout, LastBattleResult, _unlockedHulls, _unlockedUnits, _unlockedBuildings, _completedLevels);
		}
    }
}
