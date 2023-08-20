using BattleCruisers.UI.ScreensScene.ShopScreen;
using BattleCruisers.Data.Models.PrefabKeys;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using BattleCruisers.Buildables.Buildings;
using BattleCruisers.Buildables.Units;

namespace BattleCruisers.Data.Models
{
    [Serializable]
    public class SaveGameModel
    {
        // What do we need to save, critically? Just the assets and progress.

        // Number of coins I own.
        public long _coins;

        // Total historic destruction score.
        public long _lifetimeDestructionScore;

        // My callsign.
        public string _playerName;

        // My selected loadout.
        public Loadout _playerLoadout; // selected captainexo is part of this.

        // What levels have been completed
        // What difficulty those levels have been completed at.
        public Dictionary<int, int> _levelsCompleted;                          // int levelNum, enum (as int) hardestDifficulty

        // Assets owned (heckle002, captainexo001, longbow, steamcopter)
        public List<string> _unlockedHulls;                            // prefab filenames
        public Dictionary<string, string> _unlockedBuildings;          // prefab filenames, category enum strings
        public Dictionary<string, string> _unlockedUnits;              // prefab filenames, category enum strings
        public List<int> _ownedCaptainIDs;
        public List<int> _ownedHeckleIDs;

        public SaveGameModel()
        { // this is the constructor for cloud load
        }

        // Takes in GameModel, simplifies values where necessary for easier JSON parsing
        public SaveGameModel(GameModel game)
        {
            _coins = game.Coins;
            _lifetimeDestructionScore = game.LifetimeDestructionScore;
            _playerName = game.PlayerName;
            _playerLoadout = game.PlayerLoadout;
            _levelsCompleted = computeCompletedLevels(game.CompletedLevels);
            _unlockedHulls = computeUnlockedHulls(game.UnlockedHulls);
            _unlockedBuildings = computeUnlockedBuildings(game.UnlockedBuildings);
            _unlockedUnits = computeUnlockedUnits(game.UnlockedUnits);
            _ownedCaptainIDs = game.CaptainExoList;
            _ownedHeckleIDs = game.HeckleList;
        }

        // Takes in GameModel, converts and assigns values from SaveGameModel to GameModel
        public void AssignSaveToGameModel(GameModel game)
        {
            TidyUp(); // Modify null and incompatible fields before assigning

            game.Coins = _coins;
            game.LifetimeDestructionScore = _lifetimeDestructionScore;
            game.PlayerName = _playerName;
            game.PlayerLoadout = _playerLoadout;

            foreach (var level in _levelsCompleted)
            {
                CompletedLevel cLevel = new CompletedLevel(level.Key, (Settings.Difficulty)level.Value);
                game.AddCompletedLevel(cLevel);
            }

            foreach(var hull in _unlockedHulls)
            {
                HullKey hk = new HullKey(hull);
                game.AddUnlockedHull(hk);
            }

            foreach(var building in _unlockedBuildings)
            {
                // Keys and Vals are reversed here, because dictionaries require their Keys to be unique
                // these AddUnlocked constructors take an enum as their first arg, which definitionally is not unique.
                Enum.TryParse(building.Value, out BuildingCategory bc);
                BuildingKey bk = new BuildingKey(bc, building.Key);
                game.AddUnlockedBuilding(bk);
            }

            foreach (var unit in _unlockedUnits)
            {
                // Keys and Vals are reversed here, because dictionaries require their Keys to be unique
                // these AddUnlocked constructors take an enum as their first arg, which definitionally is not unique.
                Enum.TryParse(unit.Value, out UnitCategory uc);
                UnitKey uk = new UnitKey(uc, unit.Key);
                game.AddUnlockedUnit(uk);
            }

            game.CaptainExoList = _ownedCaptainIDs;
            game.HeckleList = _ownedHeckleIDs;
        }

        // Method for fixing invalid fields in saves.
        // Takes in GameModel so that it can be used to set defaults.
        private void TidyUp()
        {
            // If the captain is somehow null (because the save uses an old Loadout), set the captain to Charlie
            if(_playerLoadout.CurrentCaptain == null)
            {
                _playerLoadout.CurrentCaptain = new CaptainExoKey("CaptainExo000");
            }
        }

        private Dictionary<int, int> computeCompletedLevels(IReadOnlyCollection<CompletedLevel> levels)
        {
            var result = new Dictionary<int, int>();
            if (levels == null)
            {
                Debug.LogWarning("computeCompletedLevels returned null in SaveGameModel");
                return null;
            }
            else
            {
                foreach (var level in levels)
                {
                    result.Add(level.LevelNum, ((int)level.HardestDifficulty));
                }
            }
            return result;
        }

        private List<string> computeUnlockedHulls(IReadOnlyCollection<PrefabKeys.HullKey> hulls)
        {
            var result = new List<string>();
            if (hulls == null)
            {
                Debug.LogWarning("computeUnlockedHulls returned null in SaveGameModel");
                return null;
            }
            else
            {
                foreach (var hull in hulls)
                {
                    result.Add(hull.PrefabName);
                }
            }
            return result;
        }

        private Dictionary<string, string> computeUnlockedBuildings(IReadOnlyCollection<PrefabKeys.BuildingKey> buildings)
        {
            var result = new Dictionary<string, string>();
            if (buildings == null)
            {
                Debug.LogWarning("computeUnlockedBuildings returned null in SaveGameModel");
                return null;
            }
            else
            {
                foreach (var building in buildings)
                {
                    string category = building.BuildingCategory.ToString();
                    string prefabName = building.PrefabName;

                    result.Add(prefabName, category);
                }
            }
            return result;
        }

        private Dictionary<string, string> computeUnlockedUnits(IReadOnlyCollection<PrefabKeys.UnitKey> units)
        {
            var result = new Dictionary<string, string>();
            if (units == null)
            {
                Debug.LogWarning("computeUnlockedUnits returned null in SaveGameModel");
                return null;
            }
            else
            {
                foreach (var unit in units)
                {
                    string category = unit.UnitCategory.ToString();
                    string prefabName = unit.PrefabName;

                    result.Add(prefabName, category);
                }
            }
            return result;
        }
    }
}
