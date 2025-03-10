using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Projectiles.ActivationArgs;
using BattleCruisers.Projectiles.Stats;
using BattleCruisers.Utils.BattleScene.Pools;
using UnityEngine;

namespace BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Projectiles.Pools
{
    public class PvPMissilesFirecrackerPoolChooser : MonoBehaviour, IPvPProjectilePoolChooser<PvPRocketController, PvPTargetProviderActivationArgs<ICruisingProjectileStats>, ICruisingProjectileStats>
    {
        public IPool<PvPRocketController, PvPTargetProviderActivationArgs<ICruisingProjectileStats>> ChoosePool(IPvPProjectilePoolProvider projectilePoolProvider)
        {
            return projectilePoolProvider.MissilesFirecrackerPool;
        }
    }
}