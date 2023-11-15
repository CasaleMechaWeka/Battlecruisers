using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Buildables.Buildings.Turrets.Stats;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Projectiles.Stats;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Targets;
using UnityEngine;

namespace BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Buildables.Buildings.Turrets.BarrelControllers
{
    public interface IPvPBarrelController : IPvPTargetConsumer
    {
        Transform Transform { get; }
        IPvPTurretStats pvpTurretStats { get; }
        IPvPProjectileStats ProjectileStats { get; }
        Vector3 ProjectileSpawnerPosition { get; }
        bool IsSourceMirrored { get; }
        IPvPTarget CurrentTarget { get; }
        float BarrelAngleInDegrees { get; }

        // Basic projectiles CAN be fired without a target, as their trajectory
        // is determined by their initial velocity and gravity.  Missiles and 
        // rockets home into their target, and hence CANNOT be fired without
        // a target.
        bool CanFireWithoutTarget { get; }

        void Fire(float angleInDegrees);

        void CleanUp();
    }
}
