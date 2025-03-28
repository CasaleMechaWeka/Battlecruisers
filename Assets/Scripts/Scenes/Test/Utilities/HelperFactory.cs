using BattleCruisers.Utils;
using BattleCruisers.Utils.BattleScene.Update;
using BattleCruisers.Utils.Threading;

namespace BattleCruisers.Scenes.Test.Utilities
{
    public class HelperFactory
    {
        private const int DEFAULT_NUM_OF_DRONES = 10;

        public static Helper CreateHelper(
            int numOfDrones = DEFAULT_NUM_OF_DRONES,
            float buildSpeedMultiplier = BuildSpeedMultipliers.VERY_FAST,
            IDeferrer deferrer = null,
            IDeferrer realTimeDeferrer = null,
            IUpdaterProvider updaterProvider = null)
        {
            return
                new Helper(
                    numOfDrones,
                    buildSpeedMultiplier,
                    deferrer,
                    realTimeDeferrer,
                    updaterProvider);
        }
    }
}