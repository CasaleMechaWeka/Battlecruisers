using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Projectiles.ActivationArgs;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Projectiles.Stats;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Utils.BattleScene.Pools;
using UnityEngine;

namespace BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Projectiles.Pools
{
    public class PvPMissilesFirecrackerPoolChooser : MonoBehaviour, IPvPProjectilePoolChooser<PvPRocketController, PvPTargetProviderActivationArgs<IPvPCruisingProjectileStats>, IPvPCruisingProjectileStats>
    {
        public IPvPPool<PvPRocketController, PvPTargetProviderActivationArgs<IPvPCruisingProjectileStats>> ChoosePool(IPvPProjectilePoolProvider projectilePoolProvider)
        {
            return projectilePoolProvider.MissilesFirecrackerPool;
        }
    }
}