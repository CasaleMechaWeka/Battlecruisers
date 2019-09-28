using BattleCruisers.Projectiles.Stats;

namespace BattleCruisers.Buildables.Buildings.Turrets.AngleCalculators
{
    public class MortarAngleCalculator : GravityAffectedAngleCalculator
    {
		// Choose larger angle, because we want the mortar to fire in a high arc instead of a flat arc.
		protected override bool UseLargerAngle => true;

        public MortarAngleCalculator(IAngleHelper angleHelper, IAngleConverter angleConverter, IProjectileFlightStats projectileFlightStats) 
            : base(angleHelper, angleConverter, projectileFlightStats)
        {
        }
    }
}
