using BattleCruisers.Utils;

namespace BattleCruisers.UI.BattleScene.Clouds
{
    public abstract class CloudTeleporter : ICloudTeleporter
    {
        protected readonly ICloud _cloud;
        protected float _adjustedDisappearXPosition, _adjustedReappearXPosition;

        protected CloudTeleporter(ICloud cloud, ICloudStats cloudStats)
        {
            Helper.AssertIsNotNull(cloud, cloudStats);

            _cloud = cloud;
        }

        public abstract bool ShouldTeleportCloud();
        public abstract void TeleportCloud();
    }
}
