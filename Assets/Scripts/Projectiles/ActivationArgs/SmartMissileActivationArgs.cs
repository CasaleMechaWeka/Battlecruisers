using BattleCruisers.Buildables;
using BattleCruisers.Cruisers;
using BattleCruisers.Projectiles.Stats;
using BattleCruisers.Targets.Factories;
using BattleCruisers.Targets.TargetFinders.Filters;
using BattleCruisers.Utils;
using BattleCruisers.Utils.PlatformAbstractions.Audio;
using UnityEngine;

namespace BattleCruisers.Projectiles.ActivationArgs
{
    public class SmartMissileActivationArgs<TStats> : ProjectileActivationArgs<TStats> where TStats : IProjectileStats
    {
        public CruiserTargetFactoriesProvider TargetFactories { get; }
        public ICruiser EnemyCruiser { get; }

        public SmartMissileActivationArgs(
            Vector3 position,
            TStats projectileStats,
            Vector2 initialVelocityInMPerS,
            ITargetFilter targetFilter,
            ITarget parent,
            AudioClipWrapper impactSound,
            CruiserTargetFactoriesProvider targetFactories,
            ICruiser enemyCruiser)
            : base(position, projectileStats, initialVelocityInMPerS, targetFilter, parent, impactSound)
        {
            Helper.AssertIsNotNull(targetFactories, enemyCruiser);

            TargetFactories = targetFactories;
            EnemyCruiser = enemyCruiser;
        }
    }
}