using BattleCruisers.Projectiles.Stats;

namespace BattleCruisers.Buildables.Buildings.Turrets.AngleCalculators
{
    public class ArtilleryAngleCalculator : GravityAffectedAngleCalculator
    {
        // Choose smaller angle, because we want the artillery to fire in a flat arc instead of a high arc.
        protected override bool UseLargerAngle { get { return false; } }

        public ArtilleryAngleCalculator(IAngleHelper angleHelper, IProjectileFlightStats projectileFlightStats)
            : base(angleHelper, projectileFlightStats)
        {
        }
    }
}
