using BattleCruisers.Buildables.Buildings.Turrets.AngleCalculators;
using BattleCruisers.Projectiles.Stats;

namespace BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Buildables.Buildings.Turrets.AngleCalculators
{
    public class PvPArtilleryAngleCalculator : GravityAffectedAngleCalculator
    {
        // Choose smaller angle, because we want the artillery to fire in a flat arc instead of a high arc.
        protected override bool UseLargerAngle => false;

        public PvPArtilleryAngleCalculator(IAngleHelper angleHelper, IAngleConverter angleConverter, IProjectileFlightStats projectileFlightStats)
            : base(angleHelper, angleConverter, projectileFlightStats)
        {
        }
    }
}
