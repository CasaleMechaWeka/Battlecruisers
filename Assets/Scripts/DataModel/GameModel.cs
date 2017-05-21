using BattleCruisers.Fetchers.PrefabKeys;
using BattleCruisers.Utils;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using UnityEngine;
using UnityEngine.Assertions;

namespace BattleCruisers.DataModel
{
	public interface IGameModel
	{
		int NumOfLevelsUnlocked { get; set; }
		Loadout PlayerLoadout { get; set; }
		ReadOnlyCollection<HullKey> UnlockedHulls { get; }
		ReadOnlyCollection<BuildingKey> UnlockedBuildings { get; }
		ReadOnlyCollection<UnitKey> UnlockedUnits { get; }

		void AddUnlockedUnit(UnitKey unit);
	}

	[Serializable]
	public class GameModel : IGameModel
	{
		[SerializeField]
		private int _numOfLevelsUnlocked;

		[SerializeField]
		private Loadout _playerLoadout;

		[SerializeField]
		private List<HullKey> _unlockedHulls;

		[SerializeField]
		private List<BuildingKey> _unlockedBuildings;

		[SerializeField]
		private List<UnitKey> _unlockedUnits;

		public int NumOfLevelsUnlocked 
		{ 
			get { return _numOfLevelsUnlocked; }
			set { _numOfLevelsUnlocked = value; }
		}

		public Loadout PlayerLoadout
		{
			get { return _playerLoadout; }
			set { _playerLoadout = value; }
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

		public void AddUnlockedUnit(UnitKey unit)
		{
			Assert.IsFalse(_unlockedUnits.Contains(unit));
			_unlockedUnits.Add(unit);
		}

		public override bool Equals(object obj)
		{
			GameModel other = obj as GameModel;

			return other != null
				&& other.NumOfLevelsUnlocked == NumOfLevelsUnlocked
				&& object.ReferenceEquals(PlayerLoadout, other.PlayerLoadout)
					|| (PlayerLoadout != null && PlayerLoadout.Equals(other.PlayerLoadout))
				&& Enumerable.SequenceEqual(UnlockedHulls, other.UnlockedHulls)
				&& Enumerable.SequenceEqual(UnlockedBuildings, other.UnlockedBuildings)
				&& Enumerable.SequenceEqual(UnlockedUnits, other.UnlockedUnits);
		}

		public override int GetHashCode()
		{
			return this.GetHashCode(NumOfLevelsUnlocked, PlayerLoadout, _unlockedHulls, _unlockedUnits, _unlockedBuildings);
		}
	}
}
