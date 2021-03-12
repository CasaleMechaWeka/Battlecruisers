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
        public ICruiserTargetFactoriesProvider TargetFactories { get; }
        public ICruiser EnempCruiser { get; }

        public SmartMissileActivationArgs(
            Vector3 position,
            TStats projectileStats, 
            Vector2 initialVelocityInMPerS, 
            ITargetFilter targetFilter, 
            ITarget parent,
            IAudioClipWrapper impactSound,
            ICruiserTargetFactoriesProvider targetFactories,
            ICruiser enemyCruiser) 
            : base(position, projectileStats, initialVelocityInMPerS, targetFilter, parent, impactSound)
        {
            Helper.AssertIsNotNull(targetFactories, enemyCruiser);

            TargetFactories = targetFactories;
            EnempCruiser = enemyCruiser;
        }
    }
}