using BattleCruisers.Utils;

namespace BattleCruisers.UI.BattleScene.Clouds.Teleporters
{
    public abstract class CloudTeleporter : ICloudTeleporter
    {
        protected readonly ICloud _cloud;
        protected readonly float _disappearXPosition, _reappearXPosition;

        protected CloudTeleporter(ICloud cloud, ICloudStatsExtended cloudStats)
        {
            Helper.AssertIsNotNull(cloud, cloudStats);

            _cloud = cloud;
            _reappearXPosition = cloudStats.ReappaerLineInM;
            _disappearXPosition = cloudStats.DisappearLineInM;
        }

        public abstract bool ShouldTeleportCloud();
        public abstract void TeleportCloud();
    }
}
