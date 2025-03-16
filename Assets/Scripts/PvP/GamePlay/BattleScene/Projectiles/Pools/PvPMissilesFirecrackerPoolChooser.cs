using BattleCruisers.Projectiles.ActivationArgs;
using BattleCruisers.Projectiles.Stats;
using BattleCruisers.Utils.BattleScene.Pools;
using UnityEngine;

namespace BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Projectiles.Pools
{
    public class PvPMissilesFirecrackerPoolChooser : MonoBehaviour, IPvPProjectilePoolChooser<PvPRocketController, TargetProviderActivationArgs<ICruisingProjectileStats>, ICruisingProjectileStats>
    {
        public IPool<PvPRocketController, TargetProviderActivationArgs<ICruisingProjectileStats>> ChoosePool(IPvPProjectilePoolProvider projectilePoolProvider)
        {
            return projectilePoolProvider.MissilesFirecrackerPool;
        }
    }
}