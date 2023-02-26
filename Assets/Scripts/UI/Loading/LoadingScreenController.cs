using BattleCruisers.Data;
using BattleCruisers.Scenes;
using BattleCruisers.Utils;
using BattleCruisers.Utils.Localisation;
using UnityEngine;
using UnityEngine.Localization.Settings;
using UnityEngine.UI;

namespace BattleCruisers.UI.Loading
{
    public class LoadingScreenController : MonoBehaviour
    {
        public Canvas root;
        public Text loadingText;
        public Text SubTitle;

        private string _defaultLoadingText;
        private string startingText;

        private static bool IsFirstTime = true;
        public static LoadingScreenController Instance { get; private set; }
        public GameObject logos;

        async void Start()
        {

            Helper.AssertIsNotNull(root, loadingText);

            IApplicationModel applicationModel = ApplicationModelProvider.ApplicationModel;

            ILocTable commonStrings = await LocTableFactory.Instance.LoadCommonTableAsync();
            string subTitle = commonStrings.GetString("GameNameSubtitle").ToUpper();

#if FREE_EDITION
            //if player NOT already paid then use Free title
            if (!applicationModel.DataProvider.GameModel.PremiumEdition)
            {
                subTitle = commonStrings.GetString("GameNameFreeEdition").ToUpper();
            }
               
#else
            //if premium version set here 
            applicationModel.DataProvider.GameModel.PremiumEdition = true;
            applicationModel.DataProvider.SaveGame();
#endif

            SubTitle.text = subTitle;

            _defaultLoadingText = commonStrings.GetString("UI/LoadingScreen/DefaultLoadingText");
            startingText  = commonStrings.GetString("UI/LoadingScreen/StartingText");
            loadingText.text = FindLoadingText();
            Instance = this;

            //below is code to localise the logo
            string locName = LocalizationSettings.SelectedLocale.name;
            Transform[] ts = logos.GetComponentsInChildren<Transform>(includeInactive: true);
            foreach(Transform t in ts)
            {
                if (t.gameObject.name == locName)
                {
                    t.gameObject.SetActive(true);
                    break;
                }
            }
            IsFirstTime = false;
        }

        private string FindLoadingText()
        {
            if (IsFirstTime)
            {
                return startingText;
            }
            else
            {
                return LandingSceneGod.LoadingScreenHint ?? _defaultLoadingText;
            }
        }

        public void Destroy()
        {
            Destroy(gameObject);
        }
    }
}