using BattleCruisers.Projectiles.ActivationArgs;
using BattleCruisers.Projectiles.Stats;
using BattleCruisers.Utils.BattleScene.Pools;
using UnityEngine;

namespace BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Projectiles.Pools
{
    public class PvPMissilesMediumPoolChooser : MonoBehaviour, IPvPProjectilePoolChooser<PvPMissileController, TargetProviderActivationArgs<ProjectileStats>, ProjectileStats>
    {

        public bool isRailSlug = false;
        public Pool<PvPMissileController, TargetProviderActivationArgs<ProjectileStats>> ChoosePool(IPvPProjectilePoolProvider projectilePoolProvider)
        {
            if (isRailSlug)
            {
                return projectilePoolProvider.RailSlugsPool;
            }
            return projectilePoolProvider.MissilesMediumPool;
        }
    }
}