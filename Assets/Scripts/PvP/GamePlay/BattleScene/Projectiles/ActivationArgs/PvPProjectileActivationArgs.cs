using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Buildables;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Projectiles.Stats;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Targets.TargetFinders.Filters;
using BattleCruisers.Utils;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Utils.PlatformAbstractions.Audio;
using UnityEngine;

namespace BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Projectiles.ActivationArgs
{
    public class PvPProjectileActivationArgs<TPvPStats> where TPvPStats : IPvPProjectileStats
    {
        public Vector3 Position { get; }
        public TPvPStats ProjectileStats { get; }
        public Vector2 InitialVelocityInMPerS { get; }
        public IPvPTargetFilter TargetFilter { get; }
        public IPvPTarget Parent { get; }
        public IPvPAudioClipWrapper ImpactSound { get; }

        public PvPProjectileActivationArgs(
            Vector3 position,
            TPvPStats projectileStats,
            Vector2 initialVelocityInMPerS,
            IPvPTargetFilter targetFilter,
            IPvPTarget parent,
            IPvPAudioClipWrapper impactSound)
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