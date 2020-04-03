using BattleCruisers.UI.BattleScene.Clouds.Teleporters;
using BattleCruisers.Utils;
using BattleCruisers.Utils.BattleScene.Update;
using UnityEngine;
using UnityEngine.Assertions;

namespace BattleCruisers.UI.BattleScene.Clouds
{
    public class CloudInitialiserNEW : MonoBehaviour
    {
        private CloudTeleporterNEW _cloudTeleporter;

        public CloudControllerNEW leftCloud, rightCloud;

        public void Initialise(ICloudStats cloudStats, IUpdater updater)
        {
            Helper.AssertIsNotNull(cloudStats, updater);
            Helper.AssertIsNotNull(leftCloud, rightCloud);
            Assert.IsTrue(rightCloud.Position.x > leftCloud.Position.x);

            leftCloud.Initialise(cloudStats);
            rightCloud.Initialise(cloudStats);

            _cloudTeleporter = new CloudTeleporterNEW(updater, leftCloud, rightCloud);
        }
    }
}