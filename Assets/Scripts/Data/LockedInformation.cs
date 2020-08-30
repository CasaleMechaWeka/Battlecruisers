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
        private readonly IGameModel _gameModel;
        private readonly IStaticData _staticData;

        public const int NUM_OF_LEVELS_IN_DEMO = 7;

        public LockedInformation(IGameModel gameModel, IStaticData staticData)
        {
            Helper.AssertIsNotNull(gameModel, staticData);

            _gameModel = gameModel;
            _staticData = staticData;
        }

        public int NumOfLevelsUnlocked
        {
            get
            {
                int maxLevelCount = _staticData.IsDemo ? NUM_OF_LEVELS_IN_DEMO : _staticData.Levels.Count;
                return Mathf.Min(_gameModel.NumOfLevelsCompleted + 1, maxLevelCount);
            }
        }

        public int NumOfLockedHulls
        {
            get
            {
                return _staticData.HullKeys.Count - _gameModel.UnlockedHulls.Count;
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
            return _staticData.BuildingKeys.Count(key => key.BuildingCategory == buildingCategory);
        }

        public int NumOfLockedUnits(UnitCategory unitCategory)
        {
            int totalNumOfUnits = NumOfUnits(unitCategory);
            int numOfUnlockedUnits = _gameModel.GetUnlockedUnits(unitCategory).Count;
            return totalNumOfUnits - numOfUnlockedUnits;
        }

        private int NumOfUnits(UnitCategory unitCategory)
        {
            return _staticData.UnitKeys.Count(key => key.UnitCategory == unitCategory);
        }
    }
}
