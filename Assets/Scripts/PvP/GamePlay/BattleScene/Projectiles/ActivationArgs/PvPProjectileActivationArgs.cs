using BattleCruisers.Buildables;
using BattleCruisers.Targets.TargetFinders.Filters;
using BattleCruisers.Projectiles.Stats;
using BattleCruisers.Utils;
using BattleCruisers.Utils.PlatformAbstractions.Audio;
using UnityEngine;

namespace BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Projectiles.ActivationArgs
{
    public class PvPProjectileActivationArgs<TPvPStats> where TPvPStats : IProjectileStats
    {
        public Vector3 Position { get; }
        public TPvPStats ProjectileStats { get; }
        public Vector2 InitialVelocityInMPerS { get; }
        public ITargetFilter TargetFilter { get; }
        public ITarget Parent { get; }
        public IAudioClipWrapper ImpactSound { get; }

        public PvPProjectileActivationArgs(
            Vector3 position,
            TPvPStats projectileStats,
            Vector2 initialVelocityInMPerS,
            ITargetFilter targetFilter,
            ITarget parent,
            IAudioClipWrapper impactSound)
        {
            Helper.AssertIsNotNull(projectileStats, targetFilter, parent, impactSound);

            Position = position;
            ProjectileStats = projectileStats;
            InitialVelocityInMPerS = initialVelocityInMPerS;
            TargetFilter = targetFilter;
            Parent = parent;
            ImpactSound = impactSound;
        }
    }
}