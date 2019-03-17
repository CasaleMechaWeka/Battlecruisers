using BattleCruisers.Utils.Threading;

namespace BattleCruisers.Buildables.Buildings.Turrets.BarrelControllers
{
    public class DelayedShellTurretBarrelController : ShellTurretBarrelController
	{
        private IVariableDelayDeferrer _deferrer;

		public float delayInMs;

		public override void StaticInitialise()
        {
            base.StaticInitialise();
            _deferrer = new VariableDelayDeferrer();
        }

        public override void Fire(float angleInDegrees)
		{
            _deferrer.Defer(() => base.Fire(angleInDegrees), delayInMs / 1000);
		}
	}
}
