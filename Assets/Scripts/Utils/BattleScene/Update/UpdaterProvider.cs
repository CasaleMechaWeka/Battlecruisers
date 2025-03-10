using BattleCruisers.Utils.PlatformAbstractions.Time;
using UnityEngine;
using UnityEngine.Assertions;

namespace BattleCruisers.Utils.BattleScene.Update
{
    public class UpdaterProvider : MonoBehaviour, IUpdaterProvider
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
            PerFrameUpdater = GetComponent<PerFrameUpdater>();
            Assert.IsNotNull(PerFrameUpdater);

            PhysicsUpdater = GetComponent<PhysicsUpdater>();
            Assert.IsNotNull(PhysicsUpdater);

            SwitchableUpdater = GetComponent<SwitchableUpdater>();
            Assert.IsNotNull(SwitchableUpdater);

            SlowUpdater = new MultiFrameUpdater(PhysicsUpdater, TimeBC.Instance, SLOW_UPDATER_INTERVAL_IN_S);
            VerySlowUpdater = new MultiFrameUpdater(PhysicsUpdater, TimeBC.Instance, VERY_SLOW_UPDATER_INTERVAL_IN_S);

            BarrelControllerUpdater = PhysicsUpdater;
        }
    }
}