using UnityCommon.PlatformAbstractions.Time;
using UnityEngine;
using UnityEngine.Assertions;

namespace BattleCruisers.Utils.BattleScene.Update
{
    public class UpdaterProvider : MonoBehaviour, IUpdaterProvider
    {
        public IUpdater PerFrameUpdater { get; private set; }
        public IUpdater PhysicsUpdater { get; private set; }
        public ISwitchableUpdater SwitchableUpdater { get; private set; }

        private const float SLOWER_UPDATER_INTERVAL_IN_S = 0.2f;
        public IUpdater SlowerUpdater { get; private set; }

        // FELIX  Remove :D
        private const float BARREL_CONTROLLER_UPDATER_INTERVAL_IN_S = 0.1f;
        public IUpdater BarrelControllerUpdater { get; private set; }

        public void Initialise()
        {
            PerFrameUpdater = GetComponent<PerFrameUpdater>();
            Assert.IsNotNull(PerFrameUpdater);

            PhysicsUpdater = GetComponent<PhysicsUpdater>();
            Assert.IsNotNull(PhysicsUpdater);

            SwitchableUpdater = GetComponent<SwitchableUpdater>();
            Assert.IsNotNull(SwitchableUpdater);

            SlowerUpdater = new MultiFrameUpdater(PhysicsUpdater, TimeBC.Instance, SLOWER_UPDATER_INTERVAL_IN_S);
            // FELIX  TEMP
            BarrelControllerUpdater = PhysicsUpdater;
            //BarrelControllerUpdater = new MultiFrameUpdater(PhysicsUpdater, TimeBC.Instance, BARREL_CONTROLLER_UPDATER_INTERVAL_IN_S);
        }
    }
}