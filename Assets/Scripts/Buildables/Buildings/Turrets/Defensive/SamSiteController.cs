namespace BattleCruisers.Buildables.Buildings.Turrets.Defensive
{
    public class SamSiteController : TurretController
	{
		public override void StaticInitialise()
		{
			base.StaticInitialise();
			_attackCapabilities.Add(TargetType.Aircraft);
		}
	}
}
