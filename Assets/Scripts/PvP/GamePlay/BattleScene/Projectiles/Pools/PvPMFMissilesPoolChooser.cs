using BattleCruisers.Projectiles.ActivationArgs;
using BattleCruisers.Projectiles.Stats;
using BattleCruisers.Utils.BattleScene.Pools;
using UnityEngine;

namespace BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Projectiles.Pools
{
    public class PvPMFMissilesPoolChooser : MonoBehaviour, IPvPProjectilePoolChooser<PvPMissileController, TargetProviderActivationArgs<ProjectileStats>, ProjectileStats>
    {
        public Pool<PvPMissileController, TargetProviderActivationArgs<ProjectileStats>> ChoosePool(IPvPProjectilePoolProvider projectilePoolProvider)
        {
            return projectilePoolProvider.MissilesMFPool;
        }
    }
}