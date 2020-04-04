using BattleCruisers.Utils;
using UnityEngine;
using UnityEngine.Assertions;

namespace BattleCruisers.UI.BattleScene.Clouds.Teleporters
{
    public class TeleporterHelper : ITeleporterHelper
    {
        // Very conservative.  User probably can't see past 45, but want to make sure 
        // cloud is off screen before teleporting.
        public const float MAX_X_POSITION_VISIBLE_BY_USER = 70;

        public bool ShouldTeleportCloud(ICloud rightCloud)
        {
            Assert.IsNotNull(rightCloud);
            return rightCloud.Position.x - (rightCloud.Size.x / 2) > MAX_X_POSITION_VISIBLE_BY_USER;
        }

        public Vector2 FindTeleportTargetPosition(ICloud onScreenCloud, ICloud offScreenCloud)
        {
            Helper.AssertIsNotNull(onScreenCloud, offScreenCloud);
            Assert.IsTrue(offScreenCloud.Position.x > onScreenCloud.Position.x);

            float distanceBetweenCloudsInM = offScreenCloud.Position.x - onScreenCloud.Position.x;
            float distanceToTeleportInM = 2 * distanceBetweenCloudsInM;
            float targetPositionX = offScreenCloud.Position.x - distanceToTeleportInM;

            Vector2 targetPosition = new Vector2(targetPositionX, offScreenCloud.Position.y);
            Logging.Log(Tags.CLOUDS, $"Teleport target position: {targetPosition}");
            return targetPosition;
        }
    }
}
