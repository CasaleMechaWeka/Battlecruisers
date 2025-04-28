using BattleCruisers.Projectiles;
using BattleCruisers.Projectiles.Stats;
using BattleCruisers.Utils.BattleScene.Pools;
using UnityEngine;

namespace BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Projectiles.Pools
{
    public class PvPMissilesFirecrackerPoolChooser : MonoBehaviour, IPvPProjectilePoolChooser<PvPRocketController, ProjectileActivationArgs, ProjectileStats>
    {
        public Pool<PvPRocketController, ProjectileActivationArgs> ChoosePool(IPvPProjectilePoolProvider projectilePoolProvider)
        {
            return projectilePoolProvider.MissilesFirecrackerPool;
        }
    }
}