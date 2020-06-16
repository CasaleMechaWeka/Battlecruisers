using System.Diagnostics;
using UnityEngine;

namespace BattleCruisers.Utils.Debugging
{
    public abstract class CheaterBase : MonoBehaviour
    {
        void Awake()
        {
            DisableCheats();
        }

        // TEMP  Define DISABLE_CHEATS in final game :)
        [Conditional("DISABLE_CHEATS")]
        private void DisableCheats()
        {
            Destroy(gameObject);
        }
    }
}