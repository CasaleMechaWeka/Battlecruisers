using BattleCruisers.Buildables.Buildings.Turrets.AngleCalculators;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Projectiles.Stats;

namespace BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Buildables.Buildings.Turrets.AngleCalculators
{
    public class PvPMortarAngleCalculator : PvPGravityAffectedAngleCalculator
    {
        // Choose larger angle, because we want the mortar to fire in a high arc instead of a flat arc.
        protected override bool UseLargerAngle => true;

        public PvPMortarAngleCalculator(IAngleHelper angleHelper, IAngleConverter angleConverter, IPvPProjectileFlightStats projectileFlightStats)
            : base(angleHelper, angleConverter, projectileFlightStats)
        {
        }
    }
}
