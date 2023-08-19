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
        private Dictionary<string, string> _unlockedBuildings;          // prefab filenames, category enum strings
        private Dictionary<string, string> _unlockedUnits;              // prefab filenames, category enum strings
        private List<int> _ownedCaptainIDs;
        private List<int> _ownedHeckleIDs;
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
            _unlockedUnits = computeUnlockedUnits(game.UnlockedUnits);
            _ownedCaptainIDs = game.CaptainExoList;
            _ownedHeckleIDs = game.HeckleList;
            _ownedIAPIDs = computeOwnedIAPs(game.IAPs);
        }

        private Dictionary<int, int> computeCompletedLevels(IReadOnlyCollection<CompletedLevel> levels)
        {
            var result = new Dictionary<int, int>();
            if (levels == null)
            {
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

        private Dictionary<int, string> computeOwnedIAPs(List<IAPData> iaps)
        {
            var result = new Dictionary<int, string>();
            if (iaps == null)
            {
                return null;
            }
            else
            {
                foreach (var iap in iaps)
                {
                    result.Add(iap.IAPType, iap.IAPNameKeyBase);
                }
            }
            return result;
        }
    }
}
