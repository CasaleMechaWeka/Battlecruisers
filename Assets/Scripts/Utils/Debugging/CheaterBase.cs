using UnityEngine;

namespace BattleCruisers.Utils.Debugging
{
    public abstract class CheaterBase : MonoBehaviour
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