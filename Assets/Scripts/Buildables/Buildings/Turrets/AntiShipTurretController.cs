namespace BattleCruisers.Buildables.Buildings.Turrets
{
    public class AntiShipTurretController : TurretController
	{
		public override void StaticInitialise()
		{
			base.StaticInitialise();
			_attackCapabilities.Add(TargetType.Ships);
		}
	}
}
