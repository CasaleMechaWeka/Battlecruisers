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
        public Text onscreenLogging;

        private string _defaultLoadingText;
        private string startingText;
        public static LoadingScreenController Instance { get; private set; }
        public IApplicationModel applicationModel;
        public GameObject logos;

        async void Start()
        {

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
            Instance = this;

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
        }

        public void LogToScreen(string log)
        {
            onscreenLogging.text = log;
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



        private string FindLoadingText()
        {
            /*            if (IsFirstTime)
                        {
                            return startingText;
                        }
                        else
                        {*/
            return LandingSceneGod.LoadingScreenHint ?? startingText;
            //         }
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