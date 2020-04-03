using UnityEngine;

namespace BattleCruisers.UI.BattleScene.Clouds.Teleporters
{
    /// <summary>
    /// Clouds move left to right.  Teleport clouds to left when they go off
    /// screen on the right.
    /// </summary>
    public class PositiveVelocityTeleporter : CloudTeleporter
    {
        public PositiveVelocityTeleporter(ICloud cloud, ICloudStatsExtended cloudStats)
            : base(cloud, cloudStats)
        {
        }

        public override bool ShouldTeleportCloud()
        {
            return _cloud.Position.x > _disappearXPosition;
        }

        public override void TeleportCloud()
        {
            _cloud.Position = new Vector2(_reappearXPosition, _cloud.Position.y);
        }
    }
}
