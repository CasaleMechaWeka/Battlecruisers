using BattleCruisers.Utils;
using UnityEngine;
using UnityEngine.Assertions;

namespace BattleCruisers.UI.BattleScene.Clouds.Teleporters
{
    public class TeleporterHelper : ITeleporterHelper
    {
        // With the perspective camera now this is a little tricky.  Determined by trial and error :D
        public const float MAX_X_POSITION_VISIBLE_BY_USER = 1500;

        public bool ShouldTeleportCloud(ICloud rightCloud)
        {
            Assert.IsNotNull(rightCloud);
            return rightCloud.Position.x - (rightCloud.Size.x / 2) > MAX_X_POSITION_VISIBLE_BY_USER;
        }

        public Vector3 FindTeleportTargetPosition(ICloud onScreenCloud, ICloud offScreenCloud)
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
