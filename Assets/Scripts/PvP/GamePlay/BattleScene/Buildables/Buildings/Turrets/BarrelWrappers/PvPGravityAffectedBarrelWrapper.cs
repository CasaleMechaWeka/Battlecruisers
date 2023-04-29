using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Buildables.Buildings.Turrets.AccuracyAdjusters;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Buildables.Buildings.Turrets.AngleCalculators;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Buildables.Buildings.Turrets.AngleLimiters;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Buildables.Buildings.Turrets.BarrelControllers;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Projectiles.Stats;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Utils;
using UnityEngine;

namespace BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Buildables.Buildings.Turrets.BarrelWrappers
{
    /// <summary>
    /// Turrets:  Mortar, artillery, broadsides
    /// </summary>
    public class PvPGravityAffectedBarrelWrapper : PvPBarrelWrapper
    {
        [Header("True to use lower arc (artillery), false to use higher arc (mortar)")]
        public bool useLowerArc = true;

        protected override IPvPAngleCalculator CreateAngleCalculator(IPvPProjectileStats projectileStats)
        {
            if (useLowerArc)
            {
                return _factoryProvider.Turrets.AngleCalculatorFactory.CreateArtilleryAngleCalculator(projectileStats);
            }
            else
            {
                return _factoryProvider.Turrets.AngleCalculatorFactory.CreateMortarAngleCalculator(projectileStats);
            }
        }

        protected override IPvPAccuracyAdjuster CreateAccuracyAdjuster(IPvPAngleCalculator angleCalculator, IPvPBarrelController barrel)
        {
            if (barrel.pvpTurretStats.Accuracy >= Constants.MAX_ACCURACY)
            {
                return _factoryProvider.Turrets.AccuracyAdjusterFactory.CreateDummyAdjuster();
            }
            else
            {
                return
                    _factoryProvider.Turrets.AccuracyAdjusterFactory.CreateHorizontalImpactProjectileAdjuster(
                        angleCalculator,
                        barrel.pvpTurretStats);
            }
        }

        protected override IPvPAngleLimiter CreateAngleLimiter()
        {
            return _factoryProvider.Turrets.AngleLimiterFactory.CreateGravityAffectedLimiter();
        }
    }
}
