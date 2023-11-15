using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Projectiles.Stats;

namespace BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Buildables.Buildings.Turrets.AngleCalculators
{
    public class PvPArtilleryAngleCalculator : PvPGravityAffectedAngleCalculator
    {
        // Choose smaller angle, because we want the artillery to fire in a flat arc instead of a high arc.
        protected override bool UseLargerAngle => false;

        public PvPArtilleryAngleCalculator(IPvPAngleHelper angleHelper, IPvPAngleConverter angleConverter, IPvPProjectileFlightStats projectileFlightStats)
            : base(angleHelper, angleConverter, projectileFlightStats)
        {
        }
    }
}
