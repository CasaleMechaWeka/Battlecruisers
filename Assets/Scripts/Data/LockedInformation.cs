using System.Linq;
using BattleCruisers.Buildables.Buildings;
using BattleCruisers.Buildables.Units;
using BattleCruisers.Data.Models;
using BattleCruisers.Data.Static;
using BattleCruisers.Utils;
using UnityEngine;

namespace BattleCruisers.Data
{
    public class LockedInformation : ILockedInformation
    {
        private readonly GameModel _gameModel;

        public LockedInformation(GameModel gameModel)
        {
            Helper.AssertIsNotNull(gameModel);

            _gameModel = gameModel;
        }

        public int NumOfLevelsUnlocked
        {
            get
            {
#if IS_DEMO
                int maxLevelCount = StaticData.NUM_OF_LEVELS_IN_DEMO;
#else
                int maxLevelCount = StaticData.Levels.Count;
#endif
                return Mathf.Min(_gameModel.NumOfLevelsCompleted + 1, maxLevelCount);
            }
        }

        public int NumOfLockedHulls
        {
            get
            {
                return StaticData.HullKeys.Count - _gameModel.UnlockedHulls.Count;
            }
        }

        public int NumOfLockedBuildings(BuildingCategory buildingCategory)
        {
            int totalNumOfBuildings = NumOfBuildings(buildingCategory);
            int numOfUnlockedBuildings = _gameModel.GetUnlockedBuildings(buildingCategory).Count;
            return totalNumOfBuildings - numOfUnlockedBuildings;
        }

        private int NumOfBuildings(BuildingCategory buildingCategory)
        {
            return StaticData.BuildingKeys.Count(key => key.BuildingCategory == buildingCategory);
        }

        public int NumOfLockedUnits(UnitCategory unitCategory)
        {
            int totalNumOfUnits = NumOfUnits(unitCategory);
            int numOfUnlockedUnits = _gameModel.GetUnlockedUnits(unitCategory).Count;
            return totalNumOfUnits - numOfUnlockedUnits;
        }

        private int NumOfUnits(UnitCategory unitCategory)
        {
            return StaticData.UnitKeys.Count(key => key.UnitCategory == unitCategory);
        }
    }
}
