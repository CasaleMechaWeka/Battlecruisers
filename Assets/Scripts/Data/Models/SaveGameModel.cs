using BattleCruisers.UI.ScreensScene.ShopScreen;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;



namespace BattleCruisers.Data.Models
{
    public class SaveGameModel
    {
        // What do we need to save, critically? Just the assets and progress.

        // Number of coins I own.
        private long _coins;

        // Total historic destruction score.
        private long _lifetimeDestructionScore;

        // My callsign.
        private string _playerName;

        // My selected loadout.
        private Loadout _playerLoadout; // selected captainexo is part of this.

        // What levels have been completed
        // What difficulty those levels have been completed at.
        Dictionary<int, int> _levelsCompleted;                          // int levelNum, enum (as int) hardestDifficulty

        // Assets owned (heckle002, captainexo001, longbow, steamcopter)
        private List<string> _unlockedHulls;                            // prefab filenames
        private Dictionary<string, string> _unlockedBuildings;          //category enum strings, prefab filenames
        private Dictionary<string, string> _unlockedUnits;              //category enum strings, prefab filenames
        private List<int> _ownedCaptainIDs;                             // id is "index" in class
        private List<int> _ownedHeckleIDs;                              // id is "index" in class
        private Dictionary<int, string> _ownedIAPIDs;                   // int iapType, string iapNameKeyBase

        public SaveGameModel(GameModel game)
        {
            _coins = game.Coins;
            _lifetimeDestructionScore = game.LifetimeDestructionScore;
            _playerName = game.PlayerName;
            _playerLoadout = game.PlayerLoadout;
            _levelsCompleted = computeCompletedLevels(game.CompletedLevels);
            _unlockedHulls = computeUnlockedHulls(game.UnlockedHulls);
            _unlockedBuildings = computeUnlockedBuildings(game.UnlockedBuildings);
            //_unlockedUnits;
            //_ownedCaptainIDs;
            //_ownedHeckleIDs;
            //_ownedIAPIDs;
        }

        private Dictionary<int, int> computeCompletedLevels(IReadOnlyCollection<CompletedLevel> levels)
        {
            var result = new Dictionary<int, int>();
            foreach (var level in levels)
            {
                result.Add(level.LevelNum, ((int)level.HardestDifficulty));                
            }
            return result;
        }

        private List<string> computeUnlockedHulls(IReadOnlyCollection<PrefabKeys.HullKey> hulls)
        {
            var result = new List<string>();
            foreach (var hull in hulls)
            {
                result.Add(hull.PrefabName);
            }
            return result;
        }

        private Dictionary<string, string> computeUnlockedBuildings(IReadOnlyCollection<PrefabKeys.BuildingKey> buildings)
        {
            var result = new Dictionary<string, string>();
            foreach (var building in buildings)
            {
                result.Add(building.BuildingCategory.ToString(), building.PrefabName);
            }
            return result;
        }
    }
}
