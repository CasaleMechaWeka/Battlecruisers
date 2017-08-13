namespace BattleCruisers.Buildables.Buildings.Turrets
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
