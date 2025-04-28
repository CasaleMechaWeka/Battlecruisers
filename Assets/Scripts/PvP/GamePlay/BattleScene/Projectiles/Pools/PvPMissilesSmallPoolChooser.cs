using BattleCruisers.Projectiles;
using BattleCruisers.Projectiles.Stats;
using BattleCruisers.Utils.BattleScene.Pools;
using UnityEngine;

namespace BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Projectiles.Pools
{
    public class PvPMissilesSmallPoolChooser : MonoBehaviour, IPvPProjectilePoolChooser<PvPMissileController, ProjectileActivationArgs, ProjectileStats>
    {
        public Pool<PvPMissileController, ProjectileActivationArgs> ChoosePool(IPvPProjectilePoolProvider projectilePoolProvider)
        {
            return projectilePoolProvider.MissilesSmallPool;
        }
    }
}