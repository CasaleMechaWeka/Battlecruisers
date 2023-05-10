using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Utils.PlatformAbstractions.Time;
using UnityEngine;
using UnityEngine.Assertions;

namespace BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Utils.BattleScene.Update
{
    public class PvPUpdaterProvider : MonoBehaviour, IPvPUpdaterProvider
    {
        public IPvPUpdater PerFrameUpdater { get; private set; }
        public IPvPUpdater PhysicsUpdater { get; private set; }
        public IPvPSwitchableUpdater SwitchableUpdater { get; private set; }

        private const float SLOW_UPDATER_INTERVAL_IN_S = 0.1f;
        public IPvPUpdater SlowUpdater { get; private set; }

        private const float VERY_SLOW_UPDATER_INTERVAL_IN_S = 0.2f;
        public IPvPUpdater VerySlowUpdater { get; private set; }

        public IPvPUpdater BarrelControllerUpdater { get; private set; }


        public void Initialise()
        {
            PerFrameUpdater = GetComponent<PvPPerFrameUpdater>();
            Assert.IsNotNull(PerFrameUpdater);

            PhysicsUpdater = GetComponent<PvPPhysicsUpdater>();
            Assert.IsNotNull(PhysicsUpdater);

            SwitchableUpdater = GetComponent<PvPSwitchableUpdater>();
            Assert.IsNotNull(SwitchableUpdater);

            SlowUpdater = new PvPMultiFrameUpdater(PhysicsUpdater, PvPTimeBC.Instance, SLOW_UPDATER_INTERVAL_IN_S);
            VerySlowUpdater = new PvPMultiFrameUpdater(PhysicsUpdater, PvPTimeBC.Instance, VERY_SLOW_UPDATER_INTERVAL_IN_S);

            BarrelControllerUpdater = PhysicsUpdater;
        }
    }
}