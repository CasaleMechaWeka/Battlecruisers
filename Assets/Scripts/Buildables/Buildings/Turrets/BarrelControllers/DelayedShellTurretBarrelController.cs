using BattleCruisers.Utils.Threading;
using UnityEngine.Assertions;

namespace BattleCruisers.Buildables.Buildings.Turrets.BarrelControllers
{
    public class DelayedShellTurretBarrelController : ShellTurretBarrelController
	{
        private IDeferrer _deferrer;

		public float delayInMs;

		public override void StaticInitialise()
        {
            base.StaticInitialise();

            ConstDelayDeferrer constDelayDeferrer = GetComponent<ConstDelayDeferrer>();
            Assert.IsNotNull(constDelayDeferrer);
            constDelayDeferrer.StaticInitialise(delayInMs);
            _deferrer = constDelayDeferrer;
        }

        protected override void Fire(float angleInDegrees)
		{
            _deferrer.Defer(() => base.Fire(angleInDegrees));
		}
	}
}
