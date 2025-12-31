using BattleCruisers.Utils;
using BattleCruisers.Utils.BattleScene.Update;
using System;
using UnityEngine;
using UnityEngine.Assertions;

namespace BattleCruisers.UI.BattleScene.Clouds
{
    public class CloudTeleporter
    {
        private readonly IUpdater _updater;
        private CloudController _leftCloud, _rightCloud;

        public const float MAX_X_POSITION_VISIBLE_BY_USER = 1500;

        public CloudTeleporter(IUpdater updater, CloudController leftCloud, CloudController rightCloud)
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
            CloudController newLeftCloud = _rightCloud;
            _rightCloud = _leftCloud;
            _leftCloud = newLeftCloud;
        }

        public bool ShouldTeleportCloud(CloudController rightCloud)
        {
            Assert.IsNotNull(rightCloud);
            return rightCloud.Position.x - (rightCloud.Size.x / 2) > MAX_X_POSITION_VISIBLE_BY_USER;
        }

        public Vector3 FindTeleportTargetPosition(CloudController onScreenCloud, CloudController offScreenCloud)
        {
            Helper.AssertIsNotNull(onScreenCloud, offScreenCloud);
            Assert.IsTrue(offScreenCloud.Position.x > onScreenCloud.Position.x);

            float distanceBetweenCloudsInM = offScreenCloud.Position.x - onScreenCloud.Position.x;
            float distanceToTeleportInM = 2 * distanceBetweenCloudsInM;
            float targetPositionX = offScreenCloud.Position.x - distanceToTeleportInM;

            Vector3 targetPosition = new Vector3(targetPositionX, offScreenCloud.Position.y, offScreenCloud.Position.z);
            Logging.Log(Tags.CLOUDS, $"Teleport target position: {targetPosition}");
            return targetPosition;
        }
    }
}