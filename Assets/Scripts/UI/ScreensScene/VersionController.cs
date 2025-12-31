using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.UI;

namespace BattleCruisers.UI.ScreensScene
{
    public class VersionController : MonoBehaviour
    {
        public Text versionText;

        void Start()
        {
            Assert.IsNotNull(versionText);

            string value = "v" + Application.version;

#if IS_DEMO
                value += " - DEMO";
#endif

#if ENABLE_CHEATS
            value += " - Cheats";
#endif

#if ENABLE_LOGS
            value += " - Logs";
#endif

            //#if PSEUDO_LOCALE
            //            value += " - PseudoLoc";
            //#endif

#if UNITY_ASSERTIONS
            value += " - Asserts";
#endif

            versionText.text = value;
        }
    }
}