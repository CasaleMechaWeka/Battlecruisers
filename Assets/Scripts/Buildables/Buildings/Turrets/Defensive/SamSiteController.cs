namespace BattleCruisers.Buildables.Buildings.Turrets.Defensive
{
    public class SamSiteController : Turret
	{
		public override void StaticInitialise()
		{
			base.StaticInitialise();
			_attackCapabilities.Add(TargetType.Aircraft);
		}
	}
}
