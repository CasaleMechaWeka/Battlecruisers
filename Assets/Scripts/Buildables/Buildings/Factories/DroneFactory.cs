using System;
using BattleCruisers.Data.Static;
using BattleCruisers.UI.Sound;
using BattleCruisers.Utils.BattleScene.Update;
using BattleCruisers.Utils.Factories;
using BattleCruisers.Utils.PlatformAbstractions.Time;
using UnityEngine.Assertions;

namespace BattleCruisers.Buildables.Buildings.Factories
{
    public class DroneFactory : Building
    {
        public float DronesPerMinute;
        public int MaxDrones;

        private readonly PrioritisedSoundKey DroneCompletionSound = PrioritisedSoundKeys.Completed.Buildings.DroneStation;
        public override TargetValue TargetValue => TargetValue.Medium;

        private IUpdater updater;
        private int totalDronesProvided = 0;

        protected override void OnBuildableCompleted()
        {
            Assert.IsTrue(DronesPerMinute > 0);
            Assert.IsTrue(MaxDrones > 0);

            updater = new MultiFrameUpdater(FactoryProvider.UpdaterProvider.PhysicsUpdater,
                                            TimeBC.Instance,
                                            1 / DronesPerMinute * 60);
            updater.Updated += UpdaterUpdate;

            base.OnBuildableCompleted();
        }

        public void UpdaterUpdate(object sender, EventArgs e)
        {
            if (totalDronesProvided < MaxDrones)
            {
                ParentCruiser.DroneManager.NumOfDrones++;
                totalDronesProvided++;

                _cruiserSpecificFactories.BuildableEffectsSoundPlayer.PlaySound(DroneCompletionSound);
            }
        }

        protected override void OnDestroyed()
        {
            if (BuildableState == BuildableState.Completed)
            {
                updater.Updated -= UpdaterUpdate;
                ParentCruiser.DroneManager.NumOfDrones -= totalDronesProvided;
            }

            base.OnDestroyed();
        }
    }
}
