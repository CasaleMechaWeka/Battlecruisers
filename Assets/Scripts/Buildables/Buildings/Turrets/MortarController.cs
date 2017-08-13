namespace BattleCruisers.Buildables.Buildings.Turrets
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

