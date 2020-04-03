using UnityEngine;

namespace BattleCruisers.UI.BattleScene.Clouds.Teleporters
{
    /// <summary>
    /// Clouds move right to left.  Teleport clouds to right when they go off
    /// screen on the left.
    /// </summary>
    public class NegativeVelocityTeleporter : CloudTeleporter
    {
        public NegativeVelocityTeleporter(ICloud cloud, ICloudStatsExtended cloudStats)
            : base(cloud, cloudStats)
        {
        }

        public override bool ShouldTeleportCloud()
        {
            return _cloud.Position.x < _disappearXPosition;
        }

        public override void TeleportCloud()
        {
            _cloud.Position = new Vector2(_reappearXPosition, _cloud.Position.y);
        }
    }
}
