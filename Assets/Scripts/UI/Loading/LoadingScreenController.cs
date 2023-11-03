using System;
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

//        private static bool IsFirstTime = true;
        public static LoadingScreenController Instance { get; private set; }
        public IApplicationModel applicationModel;
        public GameObject logos;
        public Text logText;

        async void Start()
        {
            Instance = this;
            Helper.AssertIsNotNull(root, loadingText);
            applicationModel = ApplicationModelProvider.ApplicationModel;
            ILocTable commonStrings = LandingSceneGod.Instance.commonStrings;
            string subTitle = String.Empty;

            //if player NOT already paid then use Free title
            if (!applicationModel.DataProvider.GameModel.PremiumEdition)
            {
                subTitle = commonStrings.GetString("GameNameFreeEdition").ToUpper();
            }
            else if (applicationModel.DataProvider.GameModel.PremiumEdition)
            {
                subTitle = commonStrings.GetString("GameNameSubtitle").ToUpper();
            }

            SubTitle.text = subTitle;

            _defaultLoadingText = commonStrings.GetString("UI/LoadingScreen/DefaultLoadingText");
            startingText = commonStrings.GetString("UI/LoadingScreen/StartingText");
            loadingText.text = FindLoadingText();


            //below is code to localise the logo
            string locName = LocalizationSettings.SelectedLocale.name;
            Transform[] ts = logos.GetComponentsInChildren<Transform>(includeInactive: true);
            foreach (Transform t in ts)
            {
                if (t.gameObject.name == locName)
                {
                    t.gameObject.SetActive(true);
                    break;
                }
            }

/*            // get cloud data
            CloudLoad();

            IsFirstTime = false;*/
        }

        public void LogString(string str)
        {
            logText.text = str;
        }
        void Update()
        {
            /*            AudioListener[] listeners = GameObject.FindObjectsOfType<AudioListener>();
                        if (listeners.Length == 0)
                        {
                            gameObject.AddComponent<AudioListener>();
                        }
                        if (listeners.Length > 1)
                        {
                            AudioListener audioListener = gameObject.GetComponent<AudioListener>();
                            if (audioListener != null)
                            {
                                Destroy(audioListener);
                            }
                        }*/
        }

/*        private void CloudLoad()
        {
            if (IsFirstTime)
            {
                applicationModel.DataProvider.CloudLoad();
            }
        }*/

        private string FindLoadingText()
        {
/*            if (IsFirstTime)
            {
                return startingText;
            }
            else
            {*/
                return LandingSceneGod.LoadingScreenHint ?? startingText;
            // }
        }

        public void Destroy()
        {
            Destroy(gameObject);
            // enabled = false;
        }

        void OnApplicationQuit()
        {
            try
            {
                applicationModel.DataProvider.SaveGame();
                applicationModel.DataProvider.SyncCoinsToCloud();
                applicationModel.DataProvider.SyncCreditsToCloud();

                // Save changes:
                applicationModel.DataProvider.CloudSave();
            }
            catch (Exception ex)
            {
                Debug.Log(ex);
            }
        }
    }
}