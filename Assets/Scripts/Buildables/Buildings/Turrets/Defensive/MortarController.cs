using BattleCruisers.Buildables.Buildings.Turrets.AngleCalculators;

namespace BattleCruisers.Buildables.Buildings.Turrets.Defensive
{
    public class MortarController : TurretController
	{
		public override void StaticInitialise()
		{
			base.StaticInitialise();
			_attackCapabilities.Add(TargetType.Ships);
		}
	}
}

