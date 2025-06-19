using BattleCruisers.Data;
using BattleCruisers.Data.Models;
using BattleCruisers.Data.Models.PrefabKeys;
using BattleCruisers.Data.Settings;
using BattleCruisers.Data.Static;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.EventSystems;

namespace BattleCruisers.Utils.Debugging
{
    public class AdminPanel : CheaterBase, IPointerClickHandler
    {
        public GameObject buttons;

        [SerializeField]
        private int levelToUnlock = 1; // The highest level to unlock, adjustable in the Inspector


        void Start()
        {
            Assert.IsNotNull(buttons);
        }

        public void OnPointerClick(PointerEventData eventData)
        {
            if (buttons.activeSelf)
            {
                Hide();
            }
            else
            {
                Show();
            }
        }

        private void Show()
        {
            buttons.SetActive(true);
        }

        public void Hide()
        {
            buttons.SetActive(false);
        }

        public void UnlockEverything()
        {
            // Levels
            foreach (Level level in StaticData.Levels)
            {
                DataProvider.GameModel.AddCompletedLevel(new CompletedLevel(level.Num, Difficulty.Normal));
            }

            foreach (SideQuestData sideQuest in StaticData.SideQuests)
            {
                DataProvider.GameModel.AddCompletedSideQuest(new CompletedLevel(sideQuest.SideLevelNum, Difficulty.Normal));
            }

            // Hulls
            foreach (HullKey hull in StaticData.HullKeys)
            {
                if (!DataProvider.GameModel.UnlockedHulls.Contains(hull))
                {
                    DataProvider.GameModel.AddUnlockedHull(hull);
                }
            }

            // Buildings
            foreach (BuildingKey building in StaticData.BuildingKeys)
            {
                if (!DataProvider.GameModel.UnlockedBuildings.Contains(building))
                {
                    DataProvider.GameModel.AddUnlockedBuilding(building);
                    //    DataProvider.GameModel.PlayerLoadout.AddBuilding(building);
                }
            }

            // Units
            foreach (UnitKey unit in StaticData.UnitKeys)
            {
                if (!DataProvider.GameModel.UnlockedUnits.Contains(unit))
                {
                    DataProvider.GameModel.AddUnlockedUnit(unit);
                    //    DataProvider.GameModel.PlayerLoadout.AddUnit(unit);
                }
            }

            DataProvider.GameModel.HasAttemptedTutorial = true;

            // If never played a level, need to set last battle result, because levels should
            // not be unlocked without a continue result.
            if (DataProvider.GameModel.LastBattleResult == null)
            {
                DataProvider.GameModel.LastBattleResult = new BattleResult(levelNum: 1, wasVictory: false);
            }

            DataProvider.SaveGame();

            Debug.Log("Everything unlocked :D  Restart game.");
        }

        public void ResetToState()
        {
            List<int> levelsToUnlock = Enumerable.Range(1, levelToUnlock).ToList();

            // Mark tutorial as complete
            DataProvider.GameModel.HasAttemptedTutorial = true;

            // Unlock specified levels
            foreach (int levelNum in levelsToUnlock)
            {
                Level level = StaticData.Levels.FirstOrDefault(l => l.Num == levelNum);
                if (level != null)
                {
                    DataProvider.GameModel.AddCompletedLevel(new CompletedLevel(level.Num, Difficulty.Normal));
                }
                else
                {
                    SideQuestData sideQuest = StaticData.SideQuests.FirstOrDefault(sq => sq.SideLevelNum == levelNum);
                    if (sideQuest != null)
                    {
                        DataProvider.GameModel.AddCompletedSideQuest(new CompletedLevel(sideQuest.SideLevelNum, Difficulty.Normal));
                    }
                }
            }

            // Ensure last battle result is set
            if (DataProvider.GameModel.LastBattleResult == null)
            {
                DataProvider.GameModel.LastBattleResult = new BattleResult(levelNum: 1, wasVictory: false);
            }

            DataProvider.SaveGame();

            Debug.Log($"Progress reset and levels up to {levelToUnlock} unlocked. Restart game.");
        }


        public void Reset()
        {
            DataProvider.Reset();
            Debug.Log("Everything reset :D  Restart game.");
        }

        public void DvorakHotkeys()
        {
            HotkeysModel hotkeys = DataProvider.GameModel.Hotkeys;

            // Navigation
            hotkeys.PlayerCruiser = KeyCode.Semicolon;
            hotkeys.Overview = KeyCode.Q;
            hotkeys.EnemyCruiser = KeyCode.J;

            // Building categories
            hotkeys.Factories = KeyCode.A;
            hotkeys.Defensives = KeyCode.O;
            hotkeys.Offensives = KeyCode.E;
            hotkeys.Tacticals = KeyCode.U;
            hotkeys.Ultras = KeyCode.I;

            // Factories
            hotkeys.DroneStation = KeyCode.Quote;
            hotkeys.AirFactory = KeyCode.Comma;
            hotkeys.NavalFactory = KeyCode.Period;

            // Defensives
            hotkeys.ShipTurret = KeyCode.Quote;
            hotkeys.AirTurret = KeyCode.Comma;
            hotkeys.Mortar = KeyCode.Period;
            hotkeys.SamSite = KeyCode.P;
            hotkeys.TeslaCoil = KeyCode.Y;

            // Offensives
            hotkeys.Artillery = KeyCode.Quote;
            hotkeys.Railgun = KeyCode.Comma;
            hotkeys.RocketLauncher = KeyCode.Period;

            // Tacticals
            hotkeys.Shield = KeyCode.Quote;
            hotkeys.Booster = KeyCode.Comma;
            hotkeys.StealthGenerator = KeyCode.Period;
            hotkeys.SpySatellite = KeyCode.P;
            hotkeys.ControlTower = KeyCode.Y;

            // Ultras
            hotkeys.Deathstar = KeyCode.Quote;
            hotkeys.NukeLauncher = KeyCode.Comma;
            hotkeys.Ultralisk = KeyCode.Period;
            hotkeys.KamikazeSignal = KeyCode.P;
            hotkeys.Broadsides = KeyCode.Y;

            // Aircraft
            hotkeys.Bomber = KeyCode.Quote;
            hotkeys.Gunship = KeyCode.Comma;
            hotkeys.Fighter = KeyCode.Period;

            // Ships
            hotkeys.AttackBoat = KeyCode.Quote;
            hotkeys.Frigate = KeyCode.Comma;
            hotkeys.Destroyer = KeyCode.Period;
            hotkeys.Archon = KeyCode.P;

            DataProvider.SaveGame();
        }

        public void AddMoney()
        {
            DataProvider.GameModel.Coins += 1000;
            DataProvider.SaveGame();
        }

        public void RemoveMoney()
        {
            DataProvider.GameModel.Coins -= 1000;
            if (DataProvider.GameModel.Coins < 0)
            {
                DataProvider.GameModel.Coins = 0;
            }
            DataProvider.SaveGame();
        }
    }
}