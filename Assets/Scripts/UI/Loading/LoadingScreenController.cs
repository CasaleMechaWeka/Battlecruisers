using System;
using System.Collections;
using BattleCruisers.Data;
using BattleCruisers.Scenes;
using BattleCruisers.Utils;
using BattleCruisers.Utils.Localisation;
using TMPro;
using UnityEngine;
using Unity.Services.Authentication;
using UnityEngine.Localization.Settings;
using UnityEngine.UI;
using UnityEngine.Assertions;
using Unity.Services.Core;

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
        public Button idButton;
        public GameObject idHighlight;
        public AnimationClip idAnim;
        public TextMeshProUGUI idText;

        void Start()
        {
            Helper.AssertIsNotNull(root, loadingText);

            applicationModel = ApplicationModelProvider.ApplicationModel;
            string subTitle = String.Empty;

            //if player NOT already paid then use Free title
            if (!applicationModel.DataProvider.GameModel.PremiumEdition)
            {
                subTitle = LocTableCache.CommonTable.GetString("GameNameFreeEdition").ToUpper();
            }
            else if (applicationModel.DataProvider.GameModel.PremiumEdition)
            {
                subTitle = LocTableCache.CommonTable.GetString("GameNameSubtitle").ToUpper();
            }

            SubTitle.text = subTitle;

            _defaultLoadingText = LocTableCache.CommonTable.GetString("UI/LoadingScreen/DefaultLoadingText");
            startingText = LocTableCache.CommonTable.GetString("UI/LoadingScreen/StartingText");
            loadingText.text = FindLoadingText();
            Instance = this;

            Assert.IsNotNull(idButton);
            idButton.onClick.AddListener(CopyID);
#if !THIRD_PARTY_PUBLISHER
            DisplayUserID();
#endif

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

        public void CopyID()
        {
            UniClipboard.SetText(AuthenticationService.Instance.PlayerId);
            StartCoroutine(AnimateCopy());
        }

        IEnumerator AnimateCopy()
        {
            idHighlight.SetActive(true);
            yield return new WaitForSeconds(idAnim.length);
            idHighlight.SetActive(false);
        }

        private async void DisplayUserID()
        {
#if !UNITY_EDITOR
            await Task.Delay(10000);
#endif
            if (idButton != null)
                if (UnityServices.State != ServicesInitializationState.Uninitialized && AuthenticationService.Instance.PlayerId != null)
                {
                    idButton.gameObject.SetActive(true);
                    idText.text = "ID: " + AuthenticationService.Instance.PlayerId;
                }
                else
                {
                    idButton.gameObject.SetActive(false);
                }
        }
    }
}