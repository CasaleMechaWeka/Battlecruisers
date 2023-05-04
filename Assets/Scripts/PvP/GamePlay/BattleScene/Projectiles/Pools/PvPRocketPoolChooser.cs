using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Projectiles.ActivationArgs;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Projectiles.Stats;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Utils.Pools;
using UnityEngine;

namespace BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Projectiles.Pools
{
    public class PvPRocketPoolChooser : MonoBehaviour, IPvPProjectilePoolChooser<PvPRocketController, PvPTargetProviderActivationArgs<IPvPCruisingProjectileStats>, IPvPCruisingProjectileStats>
    {
        public bool isSmall = false;

        public IPvPPool<PvPRocketController, PvPTargetProviderActivationArgs<IPvPCruisingProjectileStats>>
            ChoosePool(IPvPProjectilePoolProvider projectilePoolProvider)
        {
            if (isSmall)
            {
                return projectilePoolProvider.RocketsSmallPool;
            }
            return projectilePoolProvider.RocketsPool;
        }
    }
}