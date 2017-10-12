using BattleCruisers.Utils.Threading;

namespace BattleCruisers.Buildables.Buildings.Turrets.BarrelControllers
{
    public class DelayedShellTurretBarrelController : ShellTurretBarrelController
	{
        private IDeferrer _deferrer;

		public float delayInMs;

		public override void StaticInitialise()
        {
            base.StaticInitialise();
            _deferrer = new ConstDelayDeferrer(delayInMs);
        }

        protected override void Fire(float angleInDegrees)
		{
            _deferrer.Defer(() => base.Fire(angleInDegrees));
		}
	}
}

