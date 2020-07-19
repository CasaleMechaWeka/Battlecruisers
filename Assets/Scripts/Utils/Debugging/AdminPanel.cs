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

            dataProvider.SaveGame();

            Debug.Log("Everything unlocked :D  Restart game.");
        }

        public void Reset()
        {
            ApplicationModelProvider.ApplicationModel.DataProvider.Reset();
            Debug.Log("Everything reset :D  Restart game.");
        }
    }
}