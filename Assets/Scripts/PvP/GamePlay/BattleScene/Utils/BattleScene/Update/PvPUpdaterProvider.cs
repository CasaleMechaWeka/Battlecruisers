using BattleCruisers.Utils.BattleScene.Update;
using BattleCruisers.Utils.PlatformAbstractions.Time;
using UnityEngine;
using UnityEngine.Assertions;

namespace BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Utils.BattleScene.Update
{
    public class PvPUpdaterProvider : MonoBehaviour, IUpdaterProvider
    {
        public IUpdater PerFrameUpdater { get; private set; }
        public IUpdater PhysicsUpdater { get; private set; }
        public ISwitchableUpdater SwitchableUpdater { get; private set; }

        private const float SLOW_UPDATER_INTERVAL_IN_S = 0.1f;
        public IUpdater SlowUpdater { get; private set; }

        private const float VERY_SLOW_UPDATER_INTERVAL_IN_S = 0.2f;
        public IUpdater VerySlowUpdater { get; private set; }

        public IUpdater BarrelControllerUpdater { get; private set; }


        public void Initialise()
        {
            PerFrameUpdater = GetComponent<PvPPerFrameUpdater>();
            Assert.IsNotNull(PerFrameUpdater);

            PhysicsUpdater = GetComponent<PvPPhysicsUpdater>();
            Assert.IsNotNull(PhysicsUpdater);

            SwitchableUpdater = GetComponent<PvPSwitchableUpdater>();
            Assert.IsNotNull(SwitchableUpdater);

            SlowUpdater = new PvPMultiFrameUpdater(PhysicsUpdater, TimeBC.Instance, SLOW_UPDATER_INTERVAL_IN_S);
            VerySlowUpdater = new PvPMultiFrameUpdater(PhysicsUpdater, TimeBC.Instance, VERY_SLOW_UPDATER_INTERVAL_IN_S);

            BarrelControllerUpdater = PhysicsUpdater;
        }
    }
}