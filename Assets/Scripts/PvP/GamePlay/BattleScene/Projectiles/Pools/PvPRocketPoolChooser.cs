using BattleCruisers.Projectiles.ActivationArgs;
using BattleCruisers.Projectiles.Stats;
using BattleCruisers.Utils.BattleScene.Pools;
using UnityEngine;

namespace BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Projectiles.Pools
{
    public class PvPRocketPoolChooser : MonoBehaviour, IPvPProjectilePoolChooser<PvPRocketController, TargetProviderActivationArgs<CruisingProjectileStats>, CruisingProjectileStats>
    {
        public bool isSmall = false;

        public Pool<PvPRocketController, TargetProviderActivationArgs<CruisingProjectileStats>>
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