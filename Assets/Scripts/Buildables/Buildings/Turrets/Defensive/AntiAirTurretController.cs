namespace BattleCruisers.Buildables.Buildings.Turrets.Defensive
{
    public class AntiAirTurretController : TurretController
	{
		public override void StaticInitialise()
		{
			base.StaticInitialise();
			_attackCapabilities.Add(TargetType.Aircraft);
		}
	}
}

