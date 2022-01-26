using BattleCruisers.Data;
using BattleCruisers.Data.Static;
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

            IStaticData staticData = ApplicationModelProvider.ApplicationModel.DataProvider.StaticData;

            if (staticData.IsDemo)
            {
                value += " - DEMO";
            }

#if ENABLE_CHEATS
            value += " - Cheats";
#endif

#if ENABLE_LOGS
            value += " - Logs";
#endif

//#if PSEUDO_LOCALE
//            value += " - PseudoLoc";
//#endif

            if (staticData.HasAsserts)
            {
                value += " - Asserts";
            }

            versionText.text = value;
        }
    }
}