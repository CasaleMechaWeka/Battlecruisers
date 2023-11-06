using BattleCruisers.UI.ScreensScene.ShopScreen;
using BattleCruisers.Data.Models.PrefabKeys;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Linq;
using BattleCruisers.Buildables.Buildings;
using BattleCruisers.Buildables.Units;

// Remember, this class is going to be fed into a JSON (de)serializer!
// Keep data types primitive.
// JSON has never heard of Battlecruisers, so it gets confused by our internal structures.

namespace BattleCruisers.Data.Models
{
    [Serializable]
    public class SaveGameModel
    {
        public int _saveVersion;

        // What do we need to save, critically? Just the assets and progress.

        // Total historic destruction score.
        public long _lifetimeDestructionScore;

        // My callsign.
        public string _playerName;

        // My selected loadout.
        // Selected Captain.
        // Loadout data confuses the serializer! We must map it by hand.
        public string _currentHullKey;
        public Dictionary<string, string> _currentBuildings;
        public Dictionary<string, string> _currentUnits;
        public List<int> _currentHeckles;
        public string _currentCaptain;
        public Dictionary<string, Dictionary<string, string>> _buildLimits;
        public Dictionary<string, Dictionary<string, string>> _unitLimits;
        public int _currentBodykit;
        public List<int> _selectedVariants;

        // What levels have been completed
        // What difficulty those levels have been completed at.
        public Dictionary<int, int> _levelsCompleted;                          // int levelNum, enum (as int) hardestDifficulty

        // Assets unlocked
        public List<string> _unlockedHulls;                            // prefab filenames
        public Dictionary<string, string> _unlockedBuildings;          // prefab filenames, category enum strings
        public Dictionary<string, string> _unlockedUnits;              // prefab filenames, category enum strings

        // IAPs
        public List<int> _purchasedExos;
        public List<int> _purchasedHeckles;
        public List<int> _purchasedBodykits;
        public List<int> _purchasedVariants;

        // Status tracking
        public bool _hasAttemptedTutorial;
        public bool _isDoneMigration;

        public SaveGameModel()
        { // this is the constructor for cloud load
        }

        // Takes in GameModel, simplifies values where necessary for easier JSON parsing
        public SaveGameModel(GameModel game)
        {

            // ##################################################################################
            // ##################################################################################
            //                     INCREMENT THIS IF YOU CHANGE SAVEGAMEMODEL

            _saveVersion = 4;
            // Last change: 5/11/2023

            // Consider writing handling for loading old saves with mismatched or missing fields.
            // ##################################################################################
            // ##################################################################################


            // GameModel fields:
            _lifetimeDestructionScore = game.LifetimeDestructionScore;
            _playerName = game.PlayerName;
            _levelsCompleted = computeCompletedLevels(game.CompletedLevels);
            _unlockedHulls = computeUnlockedHulls(game.UnlockedHulls);
            _unlockedBuildings = computeUnlockedBuildings(game.UnlockedBuildings);
            _unlockedUnits = computeUnlockedUnits(game.UnlockedUnits);

            // IAPs:
            _purchasedExos = game.GetExos().Distinct().ToList();
            _purchasedHeckles = game.GetHeckles().Distinct().ToList();
            _purchasedBodykits = game.GetBodykits().Distinct().ToList();
            _purchasedVariants = game.GetVariants().Distinct().ToList();

            // Loadout fields:
            _currentHullKey = game.PlayerLoadout.Hull.PrefabName;
            _currentBuildings = computeLoadoutBuildings(game.PlayerLoadout);
            _currentUnits = computeLoadoutUnits(game.PlayerLoadout);
            _currentHeckles = game.PlayerLoadout.CurrentHeckles;
            _currentCaptain = game.PlayerLoadout.CurrentCaptain.PrefabName;
            _buildLimits = computeBuildLimits(game.PlayerLoadout.GetBuildLimits());
            _unitLimits = computeUnitLimits(game.PlayerLoadout.GetUnitLimits());
            _currentBodykit = game.PlayerLoadout.SelectedBodykit;
            _selectedVariants = game.PlayerLoadout.SelectedVariants;

            // Status tracking:
            _hasAttemptedTutorial = game.HasAttemptedTutorial;
            _isDoneMigration = game.IsDoneMigration;
        }

        // Takes in GameModel, converts and assigns values from SaveGameModel to GameModel
        public void AssignSaveToGameModel(GameModel game)
        {
            game.LifetimeDestructionScore = _lifetimeDestructionScore;
            game.PlayerName = _playerName;
            game.IsDoneMigration = _isDoneMigration;

            // IAPs
            // Exos
            if (_purchasedExos != null && _purchasedExos.Count > 0)
            {
                List<int> currentList = game.GetExos();
                for (int i = 0; i <= _purchasedExos.Count - 1; i++)
                {
                    // Add
                    if (!currentList.Contains(_purchasedExos[i]))
                    {
                        game.AddExo(_purchasedExos[i]);
                        game.Captains[i].isOwned = true;
                    }
                    // Remove if they're not in the cloud save data:
                    List<int> entriesToRemove = currentList.Except(_purchasedExos).ToList();
                    foreach (int entry in entriesToRemove)
                    {
                        game.RemoveExo(entry);
                        game.Captains[entry].isOwned = false;
                    }
                }
            }
            // Heckles
            if (_purchasedHeckles != null && _purchasedHeckles.Count > 0)
            {
                List<int> currentList = game.GetHeckles();
                for (int i = 0; i <= _purchasedHeckles.Count - 1; i++)
                {
                    // Add
                    if (!currentList.Contains(_purchasedHeckles[i]))
                    {
                        game.AddHeckle(_purchasedHeckles[i]);
                    }
                    // Remove if they're not in the cloud save data:
                    List<int> entriesToRemove = currentList.Except(_purchasedHeckles).ToList();
                    foreach (int entry in entriesToRemove)
                    {
                        game.RemoveHeckle(entry);
                        game.Heckles[entry].isOwned = false;
                    }
                }
            }
            // Bodykits
            if (_purchasedBodykits != null && _purchasedBodykits.Count > 0)
            {
                List<int> currentList = game.GetBodykits();
                for (int i = 0; i <= _purchasedBodykits.Count - 1; i++)
                {
                    // Add
                    if (!currentList.Contains(_purchasedBodykits[i]))
                    {
                        game.AddBodykit(_purchasedBodykits[i]);
                    }
                    // Remove if they're not in the cloud save data:
                    List<int> entriesToRemove = currentList.Except(_purchasedBodykits).ToList();
                    foreach (int entry in entriesToRemove)
                    {
                        game.RemoveBodykit(entry);
                        game.Bodykits[entry].isOwned = false;
                    }
                }
            }
            // Variants
            if (_purchasedVariants != null && _purchasedVariants.Count > 0)
            {
                List<int> currentList = game.GetVariants();
                for (int i = 0; i <= _purchasedVariants.Count - 1; i++)
                {
                    // Add
                    if (!currentList.Contains(_purchasedVariants[i]))
                    {
                        game.AddVariant(_purchasedVariants[i]);
                    }
                    // Remove if they're not in the cloud save data:
                    List<int> entriesToRemove = currentList.Except(_purchasedVariants).ToList();
                    foreach (int entry in entriesToRemove)
                    {
                        game.RemoveVariant(entry);
                        game.Variants[entry].isOwned = false;
                    }
                }
            }

            // levels completed
            foreach (var level in _levelsCompleted)
            {
                CompletedLevel cLevel = new CompletedLevel(level.Key, (Settings.Difficulty)level.Value);
                game.AddCompletedLevel(cLevel);
            }

            // unlocked hulls
            foreach (var hull in _unlockedHulls)
            {
                HullKey hk = new HullKey(hull);
                game.AddUnlockedHull(hk);
            }

            // Keys and Vals are reversed for unlocks and current units, because dictionaries require their Keys to be
            // unique. The AddUnlocked methods take an enum as their first arg, which definitionally is not unique.

            // unlocked buildings
            foreach (var building in _unlockedBuildings)
            {
                Enum.TryParse(building.Value, out BuildingCategory bc);
                BuildingKey bk = new BuildingKey(bc, building.Key);
                game.AddUnlockedBuilding(bk);
            }

            // unlocked units
            foreach (var unit in _unlockedUnits)
            {
                Enum.TryParse(unit.Value, out UnitCategory uc);
                UnitKey uk = new UnitKey(uc, unit.Key);
                game.AddUnlockedUnit(uk);
            }

            // Loadout fields, we create a new loadout from scratch and feed it into the constructor:
            // current hull
            HullKey cHull = new HullKey(_currentHullKey);

            // current buildings
            List<BuildingKey> buildings = new List<BuildingKey>();
            foreach (var building in _currentBuildings)
            {
                Enum.TryParse(building.Value, out BuildingCategory bc);
                BuildingKey bk = new BuildingKey(bc, building.Key);
                buildings.Add(bk);
            }

            // current units
            List<UnitKey> units = new List<UnitKey>();
            foreach (var unit in _unlockedUnits)
            {
                Enum.TryParse(unit.Value, out UnitCategory uc);
                UnitKey uk = new UnitKey(uc, unit.Key);
                units.Add(uk);
            }

            // building limits
            // the data structure here is pretty tough to process.
            Dictionary<BuildingCategory, List<BuildingKey>> buildLimits = new Dictionary<BuildingCategory, List<BuildingKey>>();
            foreach (string buildCat in _buildLimits.Keys)
            {
                Enum.TryParse(buildCat, out BuildingCategory bc);

                Dictionary<string, string> buildingKeys;
                _buildLimits.TryGetValue(buildCat, out buildingKeys);

                List<BuildingKey> parsedBuildingKeys = new List<BuildingKey>();
                foreach (var buildKey in buildingKeys)
                {
                    Enum.TryParse(buildKey.Value, out BuildingCategory keycat);
                    BuildingKey newKey = new BuildingKey(keycat, buildKey.Key);
                    parsedBuildingKeys.Add(newKey);
                }
                buildLimits.Add(bc, parsedBuildingKeys);
            }

            // unit limits
            // the data structure here is pretty tough to process.
            Dictionary<UnitCategory, List<UnitKey>> unitLimits = new Dictionary<UnitCategory, List<UnitKey>>();
            foreach (string unitCat in _unitLimits.Keys)
            {
                Enum.TryParse(unitCat, out UnitCategory uc);

                Dictionary<string, string> unitKeys;
                _unitLimits.TryGetValue(unitCat, out unitKeys);

                List<UnitKey> parsedUnitKeys = new List<UnitKey>();
                foreach (var unitKey in unitKeys)
                {
                    Enum.TryParse(unitKey.Value, out UnitCategory keycat);
                    UnitKey newKey = new UnitKey(keycat, unitKey.Key);
                    parsedUnitKeys.Add(newKey);
                }
                unitLimits.Add(uc, parsedUnitKeys);
            }

            // loadout construction actually happens finally:
            game.PlayerLoadout = new Loadout(cHull, buildings, units, buildLimits, unitLimits, game);

            // current variants
            if (_selectedVariants != null)
            {
                game.PlayerLoadout.SelectedVariants = _selectedVariants;
            }
            else
            {
                game.PlayerLoadout.SelectedVariants = new List<int>();
            }

            // current bodykit
            if (_currentBodykit == -1 || game.Bodykits[_currentBodykit].isOwned)
            {
                game.PlayerLoadout.SelectedBodykit = _currentBodykit;
            }
            else
            {
                game.PlayerLoadout.SelectedBodykit = -1;
            }

            // current heckles
            if (_currentHeckles != null)
            {
                game.PlayerLoadout.CurrentHeckles = _currentHeckles;
                foreach (int i in _currentHeckles)
                    game._heckles[i].isOwned = true;
            }
            else
            {
                game.PlayerLoadout.CurrentHeckles = computeUnlockedHeckles(game);
            }

            // current captain
            if (_currentCaptain != null)
            {
                game.PlayerLoadout.CurrentCaptain = new CaptainExoKey(_currentCaptain);
            }
            else
            {
                game.PlayerLoadout.CurrentCaptain = new CaptainExoKey("CaptainExo000");
            }

            // Tutorial status check
            if (_levelsCompleted.ContainsKey(1) || _hasAttemptedTutorial == true)
            {
                game.HasAttemptedTutorial = true;
            }
            else
            {
                game.HasAttemptedTutorial = false;
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

        private List<int> computeUnlockedHeckles(GameModel game)
        {
            List<int> unlockedHeckles = new List<int>();
            int numHecklesUnlocked = 3;
            while (unlockedHeckles.Count < numHecklesUnlocked)
            {
                int unlockHeckle = UnityEngine.Random.Range(0, 279);
                if (!unlockedHeckles.Contains(unlockHeckle))
                {
                    game._heckles[unlockHeckle].isOwned = true;
                    unlockedHeckles.Add(unlockHeckle);
                }
            }
            return unlockedHeckles;
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

        private Dictionary<string, string> computeLoadoutBuildings(Loadout loadout)
        {
            var result = new Dictionary<string, string>();

            foreach (BuildingKey building in loadout.GetBuildings(BuildingCategory.Factory))
            {
                string category = BuildingCategory.Factory.ToString();
                string prefabName = building.PrefabName;

                result.Add(prefabName, category);
            }

            foreach (BuildingKey building in loadout.GetBuildings(BuildingCategory.Defence))
            {
                string category = BuildingCategory.Defence.ToString();
                string prefabName = building.PrefabName;

                result.Add(prefabName, category);
            }

            foreach (BuildingKey building in loadout.GetBuildings(BuildingCategory.Offence))
            {
                string category = BuildingCategory.Offence.ToString();
                string prefabName = building.PrefabName;

                result.Add(prefabName, category);
            }

            foreach (BuildingKey building in loadout.GetBuildings(BuildingCategory.Tactical))
            {
                string category = BuildingCategory.Tactical.ToString();
                string prefabName = building.PrefabName;

                result.Add(prefabName, category);
            }

            foreach (BuildingKey building in loadout.GetBuildings(BuildingCategory.Ultra))
            {
                string category = BuildingCategory.Ultra.ToString();
                string prefabName = building.PrefabName;

                result.Add(prefabName, category);
            }

            return result;
        }

        private Dictionary<string, string> computeLoadoutUnits(Loadout loadout)
        {
            var result = new Dictionary<string, string>();

            foreach (UnitKey unit in loadout.GetUnits(UnitCategory.Aircraft))
            {
                string category = UnitCategory.Aircraft.ToString();
                string prefabName = unit.PrefabName;

                result.Add(prefabName, category);
            }

            foreach (UnitKey unit in loadout.GetUnits(UnitCategory.Naval))
            {
                string category = UnitCategory.Naval.ToString();
                string prefabName = unit.PrefabName;

                result.Add(prefabName, category);
            }

            return result;
        }

        private Dictionary<string, Dictionary<string, string>> computeBuildLimits(Dictionary<BuildingCategory, List<BuildingKey>> buildLimits)
        {
            var result = new Dictionary<string, Dictionary<string, string>>();

            foreach (BuildingCategory cat in buildLimits.Keys)
            {
                string category = cat.ToString();

                List<BuildingKey> buildings;
                buildLimits.TryGetValue(cat, out buildings);

                Dictionary<string, string> parsedBuildings = new Dictionary<string, string>();
                foreach (BuildingKey unparsedBuilding in buildings)
                {
                    string localCategory = unparsedBuilding.BuildingCategory.ToString();
                    string prefabName = unparsedBuilding.PrefabName;
                    parsedBuildings.Add(prefabName, localCategory);
                }
                result.Add(category, parsedBuildings);
            }

            return result;
        }

        private Dictionary<string, Dictionary<string, string>> computeUnitLimits(Dictionary<UnitCategory, List<UnitKey>> unitLimits)
        {
            var result = new Dictionary<string, Dictionary<string, string>>();

            foreach (UnitCategory cat in unitLimits.Keys)
            {
                string category = cat.ToString();

                List<UnitKey> units;
                unitLimits.TryGetValue(cat, out units);

                Dictionary<string, string> parsedUnits = new Dictionary<string, string>();
                foreach (UnitKey unparsedUnit in units)
                {
                    string localCategory = unparsedUnit.UnitCategory.ToString();
                    string prefabName = unparsedUnit.PrefabName;
                    parsedUnits.Add(prefabName, localCategory);
                }
                result.Add(category, parsedUnits);
            }

            return result;
        }
    }
}
