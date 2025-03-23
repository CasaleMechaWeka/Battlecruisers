using BattleCruisers.Projectiles.ActivationArgs;
using BattleCruisers.Projectiles.Stats;
using BattleCruisers.Utils.BattleScene.Pools;
using UnityEngine;

namespace BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Projectiles.Pools
{
    public class PvPMissilesSmallPoolChooser : MonoBehaviour, IPvPProjectilePoolChooser<PvPMissileController, TargetProviderActivationArgs<IProjectileStats>, IProjectileStats>
    {
        public Pool<PvPMissileController, TargetProviderActivationArgs<IProjectileStats>> ChoosePool(IPvPProjectilePoolProvider projectilePoolProvider)
        {
            return projectilePoolProvider.MissilesSmallPool;
        }
    }
}