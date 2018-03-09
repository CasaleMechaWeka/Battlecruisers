using UnityEngine;

namespace BattleCruisers.UI.BattleScene.Clouds.Teleporters
{
    /// <summary>
    /// Clouds move left to right.  Teleport clouds to left when they go off
    /// screen on the right.
    /// </summary>
    public class PositiveVelocityTeleporter : CloudTeleporter
    {
        public PositiveVelocityTeleporter(ICloud cloud, ICloudStats cloudStats)
            : base(cloud, cloudStats)
        {
            _adjustedReappearXPosition = cloudStats.ReappaerLineInM - _cloud.Size.x;
            _adjustedDisappearXPosition = cloudStats.DisappearLineInM + _cloud.Size.x;
        }

        public override bool ShouldTeleportCloud()
        {
            return _cloud.Position.x > _adjustedDisappearXPosition;
        }

        public override void TeleportCloud()
        {
            _cloud.Position = new Vector2(_adjustedReappearXPosition, _cloud.Position.y);
        }
    }
}
