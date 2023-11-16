using BattleCruisers.Data;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Utils;
using UnityEngine.Assertions;

namespace BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Buildables.BuildProgress
{
    /// <summary>
    /// Calculates build progress linearly.  This is the "normal" build progress,
    /// and how build progress should be calculated in game.
    /// </summary>
    public class PvPLinearCalculator : IPvPBuildProgressCalculator
    {
        private readonly float _buildMultiplier;

        public PvPLinearCalculator(float buildSpeedMultiplier = PvPBuildSpeedMultipliers.DEFAULT)
        {
            // Logging.Log(Tags.BUILD_PROGRESS, $"build speed multiplier: {buildSpeedMultiplier}");

            IApplicationModel applicationModel = ApplicationModelProvider.ApplicationModel;
            //should be enabled in Production
            _buildMultiplier = buildSpeedMultiplier;

            //if (applicationModel.Mode == GameMode.PvP_1VS1 && applicationModel.DataProvider.GameModel.GameMap == 4)
            //{
            //    _buildMultiplier = 2;
            //}
            //else if (applicationModel.Mode == GameMode.PvP_1VS1 && applicationModel.DataProvider.GameModel.GameMap == 6)
            //{
            //    _buildMultiplier = 3;
            //}
            //else if (applicationModel.Mode == GameMode.PvP_1VS1 && applicationModel.DataProvider.GameModel.GameMap == 8)
            //{
            //    _buildMultiplier = 4;
            //}
            //else if (applicationModel.Mode == GameMode.PvP_1VS1 && applicationModel.DataProvider.GameModel.GameMap == 9)
            //{
            //    _buildMultiplier = 5;
            //}
            //else
            //{
            //    _buildMultiplier = buildSpeedMultiplier;
            //}

            // cheat code 
            // _buildMultiplier = 10;
        }

        public float CalculateBuildProgressInDroneS(IPvPBuildable buildableUnderConstruction, float deltaTime)
        {
            Assert.IsTrue(buildableUnderConstruction.BuildableState == PvPBuildableState.InProgress);
            // Logging.Log(Tags.BOOST, $"Boost multiplier: {buildableUnderConstruction.BuildProgressBoostable.BoostMultiplier}");

            return
                buildableUnderConstruction.DroneConsumer.NumOfDrones
                * buildableUnderConstruction.BuildProgressBoostable.BoostMultiplier
                * deltaTime
                * _buildMultiplier;
        }
    }
}