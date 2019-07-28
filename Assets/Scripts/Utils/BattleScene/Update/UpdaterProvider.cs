using UnityCommon.PlatformAbstractions;
using UnityEngine;
using UnityEngine.Assertions;

namespace BattleCruisers.Utils.BattleScene.Update
{
    public class UpdaterProvider : MonoBehaviour, IUpdaterProvider
    {
        public IUpdater PerFrameUpdater { get; private set; }

        private const float SLOWER_UPDATER_INTERVAL_IN_S = 0.2f;
        public IUpdater SlowerUpdater { get; private set; }
        
        public void Initialise()
        {
            PerFrameUpdater = GetComponent<PerFrameUpdater>();
            Assert.IsNotNull(PerFrameUpdater);

            SlowerUpdater = new MultiFrameUpdater(PerFrameUpdater, new TimeBC(), SLOWER_UPDATER_INTERVAL_IN_S);
        }
    }
}