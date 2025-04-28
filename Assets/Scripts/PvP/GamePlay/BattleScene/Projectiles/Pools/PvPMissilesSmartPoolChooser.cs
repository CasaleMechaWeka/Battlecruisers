using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Projectiles.ActivationArgs;
using BattleCruisers.Projectiles.Stats;
using BattleCruisers.Utils.BattleScene.Pools;
using UnityEngine;

namespace BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Projectiles.Pools
{
    public class PvPMissilesSmartPoolChooser : MonoBehaviour, IPvPProjectilePoolChooser<PvPSmartMissileController, PvPSmartMissileActivationArgs<ProjectileStats>, ProjectileStats>
    {
        public Pool<PvPSmartMissileController, PvPSmartMissileActivationArgs<ProjectileStats>> ChoosePool(IPvPProjectilePoolProvider projectilePoolProvider)
        {
            return projectilePoolProvider.MissilesSmartPool;
        }
    }
}