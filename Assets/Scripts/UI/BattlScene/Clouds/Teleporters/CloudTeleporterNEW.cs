using BattleCruisers.Utils;
using BattleCruisers.Utils.BattleScene.Update;
using System;
using UnityEngine;
using UnityEngine.Assertions;

namespace BattleCruisers.UI.BattleScene.Clouds.Teleporters
{
    // FELIX  Rename all NEWs :)
    // FELIX Test?
    public class CloudTeleporterNEW
    {
        // FELIX  Use slower updater!  Don't need to check every frame :)
        private readonly IUpdater _updater;
        private ICloudNEW _leftCloud, _rightCloud;

        // Very conservative.  User probably can't see past 45, but want to make sure 
        // cloud is off screen before teleporting.
        private const float MAX_X_POSITION_VISIBLE_BY_USER = 70;
        private const float CLOUD_GAP_IN_M = 1;

        public CloudTeleporterNEW(IUpdater updater, ICloudNEW leftCloud, ICloudNEW rightCloud)
        {
            Helper.AssertIsNotNull(updater, leftCloud, rightCloud);
            Assert.IsTrue(leftCloud.Position.x < rightCloud.Position.x);

            _updater = updater;
            _leftCloud = leftCloud;
            _rightCloud = rightCloud;

            _updater.Updated += _updater_Updated;
        }

        private void _updater_Updated(object sender, EventArgs e)
        {
            if (!ShouldTeleportCloud(_rightCloud))
            {
                return;
            }

            _rightCloud.Position = FindTeleportTargetPosition(_leftCloud, _rightCloud);

            // Switch clouds
            ICloudNEW newLeftCloud = _rightCloud;
            _rightCloud = _leftCloud;
            _leftCloud = newLeftCloud;
        }

        // FELIX  Abstract & test :)
        private bool ShouldTeleportCloud(ICloudNEW rightCloud)
        {
            Assert.IsNotNull(rightCloud);
            return rightCloud.Position.x - (rightCloud.Size.x / 2) > MAX_X_POSITION_VISIBLE_BY_USER;
        }

        // FELIX  Abstract & test :)
        private Vector2 FindTeleportTargetPosition(ICloudNEW onScreenCloud, ICloudNEW offScreenCloud)
        {
            Helper.AssertIsNotNull(onScreenCloud, offScreenCloud);
            Assert.IsTrue(offScreenCloud.Position.x > onScreenCloud.Position.x);

            float distanceBetweenCloudsInM = onScreenCloud.Size.x / 2 + CLOUD_GAP_IN_M + offScreenCloud.Size.x / 2;
            float targetPositionX = onScreenCloud.Position.x - distanceBetweenCloudsInM;

            Vector2 targetPosition = new Vector2(targetPositionX, offScreenCloud.Position.y);
            Logging.Log(Tags.CLOUDS, $"Teleport target position: {targetPosition}");
            return targetPosition;
        }
    }
}
