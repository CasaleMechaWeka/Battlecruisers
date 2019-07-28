using UnityCommon.PlatformAbstractions;
using UnityEngine;
using UnityEngine.Assertions;

namespace BattleCruisers.Utils.BattleScene.Update
{
    public class UpdaterProvider : MonoBehaviour, IUpdaterProvider
    {
        public IUpdater PerFrameUpdater { get; private set; }
        public IUpdater PhysicsUpdater { get; private set; }

        private const float SLOWER_UPDATER_INTERVAL_IN_S = 0.2f;
        public IUpdater SlowerUpdater { get; private set; }

        public void Initialise()
        {
            PerFrameUpdater = GetComponent<PerFrameUpdater>();
            Assert.IsNotNull(PerFrameUpdater);

            PhysicsUpdater = GetComponent<PhysicsUpdater>();
            Assert.IsNotNull(PhysicsUpdater);

            SlowerUpdater = new MultiFrameUpdater(PhysicsUpdater, new TimeBC(), SLOWER_UPDATER_INTERVAL_IN_S);
        }
    }
}