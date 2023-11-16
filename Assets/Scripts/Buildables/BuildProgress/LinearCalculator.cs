using BattleCruisers.Data;
using BattleCruisers.Data.Settings;
using BattleCruisers.Utils;
using UnityEngine.Assertions;

namespace BattleCruisers.Buildables.BuildProgress
{
    /// <summary>
    /// Calculates build progress linearly.  This is the "normal" build progress,
    /// and how build progress should be calculated in game.
    /// </summary>
    public class LinearCalculator : IBuildProgressCalculator
    {
        private readonly float _buildMultiplier;
        private ISettingsManager settingsManager;

        public LinearCalculator(float buildSpeedMultiplier = BuildSpeedMultipliers.DEFAULT)
        {
            Logging.Log(Tags.BUILD_PROGRESS, $"build speed multiplier: {buildSpeedMultiplier}");
            //_buildMultiplier = buildSpeedMultiplier;

            // TURBO MODE FOR PREMIUM (ONLY FOR PVE!!!)
            IApplicationModel applicationModel = ApplicationModelProvider.ApplicationModel;
            settingsManager = applicationModel.DataProvider.SettingsManager;

            if(settingsManager.TurboMode)
            {
                _buildMultiplier = 10;
            }
            //else if (applicationModel.Mode == GameMode.PvP_1VS1 && applicationModel.DataProvider.GameModel.GameMap == 4)
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
            else
            {
                _buildMultiplier = buildSpeedMultiplier;
            }
        }

        public float CalculateBuildProgressInDroneS(IBuildable buildableUnderConstruction, float deltaTime)
        {
            Assert.IsTrue(buildableUnderConstruction.BuildableState == BuildableState.InProgress);
            Logging.Log(Tags.BOOST, $"Boost multiplier: {buildableUnderConstruction.BuildProgressBoostable.BoostMultiplier}");

            return 
                buildableUnderConstruction.DroneConsumer.NumOfDrones 
                * buildableUnderConstruction.BuildProgressBoostable.BoostMultiplier 
                * deltaTime 
                * _buildMultiplier;
        }
    }
}