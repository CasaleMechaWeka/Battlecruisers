using System.Linq;
using BattleCruisers.Buildables.Buildings;
using BattleCruisers.Buildables.Units;
using BattleCruisers.Data.Models;
using BattleCruisers.Data.Static;
using BattleCruisers.Utils;
using UnityEngine;

namespace BattleCruisers.Data
{
    // FELIX  Test :D
    public class LockedInformation : ILockedInformation
    {
        private readonly IGameModel _gameModel;
        private readonly IStaticData _staticData;

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
                return Mathf.Min(_gameModel.NumOfLevelsCompleted + 1, _staticData.Levels.Count);
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
