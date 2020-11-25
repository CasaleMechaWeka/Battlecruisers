using BattleCruisers.Data;
using BattleCruisers.Data.Models;
using BattleCruisers.Data.Models.PrefabKeys;
using BattleCruisers.Data.Settings;
using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.EventSystems;

namespace BattleCruisers.Utils.Debugging
{
    public class AdminPanel : CheaterBase, IPointerClickHandler
    {
        public GameObject buttons;

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
            IDataProvider dataProvider = ApplicationModelProvider.ApplicationModel.DataProvider;

            // Levels
            foreach (ILevel level in dataProvider.Levels)
            {
                dataProvider.GameModel.AddCompletedLevel(new CompletedLevel(level.Num, Difficulty.Normal));
            }

            // Hulls
            foreach (HullKey hull in dataProvider.StaticData.HullKeys)
            {
                if (!dataProvider.GameModel.UnlockedHulls.Contains(hull))
                {
                    dataProvider.GameModel.AddUnlockedHull(hull);
                }
            }

            // Buildings
            foreach (BuildingKey building in dataProvider.StaticData.BuildingKeys)
            {
                if (!dataProvider.GameModel.UnlockedBuildings.Contains(building))
                {
                    dataProvider.GameModel.AddUnlockedBuilding(building);
                    dataProvider.GameModel.PlayerLoadout.AddBuilding(building);
                }
            }

            // Units
            foreach (UnitKey unit in dataProvider.StaticData.UnitKeys)
            {
                if (!dataProvider.GameModel.UnlockedUnits.Contains(unit))
                {
                    dataProvider.GameModel.AddUnlockedUnit(unit);
                    dataProvider.GameModel.PlayerLoadout.AddUnit(unit);
                }
            }

            dataProvider.GameModel.HasAttemptedTutorial = true;

            // If never played a level, need to set last battle result, because levels should
            // not be unlocked without a continue result.
            if (dataProvider.GameModel.LastBattleResult == null)
            {
                dataProvider.GameModel.LastBattleResult = new BattleResult(levelNum: 1, wasVictory: false);
            }

            dataProvider.SaveGame();

            Debug.Log("Everything unlocked :D  Restart game.");
        }

        public void Reset()
        {
            ApplicationModelProvider.ApplicationModel.DataProvider.Reset();
            Debug.Log("Everything reset :D  Restart game.");
        }

        public void DvorakHotkeys()
        {
            IDataProvider dataProvider = ApplicationModelProvider.ApplicationModel.DataProvider;
            IHotkeysModel hotkeys = dataProvider.GameModel.Hotkeys;

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

            dataProvider.SaveGame();
        }
    }
}