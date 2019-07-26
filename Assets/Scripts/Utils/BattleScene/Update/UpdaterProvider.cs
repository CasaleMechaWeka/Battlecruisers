using UnityCommon.PlatformAbstractions;
using UnityEngine;
using UnityEngine.Assertions;

namespace BattleCruisers.Utils.BattleScene.Update
{
    public class UpdaterProvider : MonoBehaviour, IUpdaterProvider
    {
        private IUpdater _perFrameUpdater;

        private const float SLOWER_UPDATER_INTERVAL_IN_S = 0.2f;
        public IUpdater SlowerUpdater { get; private set; }
        
        public void Initialise()
        {
            _perFrameUpdater = GetComponent<PerFrameUpdater>();
            Assert.IsNotNull(_perFrameUpdater);

            SlowerUpdater = new MultiFrameUpdater(_perFrameUpdater, new TimeBC(), SLOWER_UPDATER_INTERVAL_IN_S);
        }
    }
}