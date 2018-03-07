using UnityEngine;

namespace BattleCruisers.UI.BattleScene.Clouds
{
    /// <summary>
    /// Clouds move right to left.  Teleport clouds to right when they go off
    /// screen on the left.
    /// </summary>
    public class NegativeVelocityTeleporter : CloudTeleporter
    {
        public NegativeVelocityTeleporter(ICloud cloud, ICloudStats cloudStats)
            : base(cloud, cloudStats)
        {
            _adjustedReappearXPosition = cloudStats.ReappaerLineInM + _cloud.Size.x;
            _adjustedDisappearXPosition = cloudStats.DisappearLineInM - _cloud.Size.x;
        }

        public override bool ShouldTeleportCloud()
        {
            return _cloud.Position.x < _adjustedDisappearXPosition;
        }

        public override void TeleportCloud()
        {
            _cloud.Position = new Vector2(_adjustedReappearXPosition, _cloud.Position.y);
        }
    }
}
