namespace BattleCruisers.Buildables.Buildings.Turrets.Defensive
{
    public class AntiAirTurretController : Turret
	{
		public override void StaticInitialise()
		{
			base.StaticInitialise();
			_attackCapabilities.Add(TargetType.Aircraft);
		}
	}
}

