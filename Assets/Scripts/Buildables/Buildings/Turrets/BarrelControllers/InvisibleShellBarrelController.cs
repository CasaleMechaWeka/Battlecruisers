namespace BattleCruisers.Buildables.Buildings.Turrets.BarrelControllers
{
	/// <summary>
	/// Do not have a barrel to move.
	/// 
	/// Decide if we are on target by checking if the target is within the firing zone.
	/// 
	/// FELIX  Implement firing zone :P  Maybe?  Hm.
	/// </summary>
	public class InvisibleShellBarrelController : ShellTurretBarrelController 
	{
		protected override bool IsOnTarget(float desiredAngleInDegrees)
		{
			return true;
		}

		protected override void AdjustBarrel(float desiredAngleInDegrees) { }
	}
}
