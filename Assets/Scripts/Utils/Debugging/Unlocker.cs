using BattleCruisers.Data;
using BattleCruisers.Data.Models;
using BattleCruisers.Data.Models.PrefabKeys;
using BattleCruisers.Data.Settings;
using UnityEngine;
using UnityEngine.EventSystems;

namespace BattleCruisers.Utils.Debugging
{
    public class Unlocker : MonoBehaviour, IPointerClickHandler
    {
        private int _numOfClicks = 0;

        public int numOfClicksToUnlock = 7;

        public void OnPointerClick(PointerEventData eventData)
        {
            _numOfClicks++;
            Debug.Log($"{_numOfClicks}/{numOfClicksToUnlock}");

            if (_numOfClicks == numOfClicksToUnlock)
            {
                UnlockEverything();
                Debug.Log("Everything's unlocked :P");
                enabled = false;
            }
        }

        private void UnlockEverything()
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
                }
            }

            // Units
            foreach (UnitKey unit in dataProvider.StaticData.UnitKeys)
            {
                if (!dataProvider.GameModel.UnlockedUnits.Contains(unit))
                {
                    dataProvider.GameModel.AddUnlockedUnit(unit);
                }
            }

            dataProvider.SaveGame();
        }
    }
}