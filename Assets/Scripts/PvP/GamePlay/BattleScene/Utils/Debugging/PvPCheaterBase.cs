using UnityEngine;

namespace BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Utils.Debugging
{
    public abstract class PvPCheaterBase : MonoBehaviour
    {
        void Awake()
        {
            // TEMP  Remove define symbol from final build :)
#if !ENABLE_CHEATS
            Destroy(gameObject);
#endif
        }
    }
}