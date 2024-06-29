using BattleCruisers.Data;
using BattleCruisers.Data.Helpers;
using BattleCruisers.Data.Static;
using BattleCruisers.Scenes;
using UnityEngine.UI;
using BattleCruisers.UI.ScreensScene.ProfileScreen;
using BattleCruisers.UI.Sound.Players;
using BattleCruisers.Utils;
using BattleCruisers.Utils.Fetchers;
using BattleCruisers.Utils.Localisation;
using System.Threading.Tasks;
using UnityEngine;
using System;
using System.Collections.Generic;
using Unity.Services.Core;
using BattleCruisers.Data.Models.PrefabKeys;
using System.Collections;

namespace BattleCruisers.UI.ScreensScene.BattleHubScreen
{
    public class ShopPanelScreenController : ScreenController
    {
        public CanvasGroupButton backButton, /*buyCaptainButton, buyHeckleButton,*/ blackMarketButton, infoButton;
        public CanvasGroupButton captainsButton, hecklesButton, bodykitButton, variantsButton;
        public Transform captainItemContainer, heckleItemContainer, bodykitItemContainer, variantsItemContainer;
        public GameObject captainItemPrefab, heckleItemPrefab, bodykitItemPrefab, variantItemPrefab;
        public CaptainsContainer captainsContainer;
        public HecklesContainer hecklesContainer;
        public BodykitsContainer bodykitsContainer;
        public VariantsContainer variantsContainer;
        public GameObject hecklesMessage;
        private IPrefabFactory _prefabFactory;
        private ISingleSoundPlayer _soundPlayer;
        private IDataProvider _dataProvider;
        public Transform captainCamContainer;
        private ILocTable commonStrings;
        public Image captainsButtonImage, hecklesButtonImage, bodyKitButtonImage, variantButtonImage;
        public Text blackMarketText;
        private bool InternetConnection;
        private Color32 navButtonActive = new Color32(255, 255, 255, 255);
        private Color32 navButtonInactive = new Color32(194, 59, 33, 255);

        private List<int> variantList;
        private List<VariantPrefab> variants = new List<VariantPrefab>();

        private List<int> bodykitList;
        private List<Bodykit> bodykits = new List<Bodykit>();

        private List<int> exoBaseList;
        private List<CaptainExo> captains = new List<CaptainExo>();

        public async Task Initialise(
            IScreensSceneGod screensSceneGod,
            ISingleSoundPlayer soundPlayer,
            IPrefabFactory prefabFactory,
            IDataProvider dataProvider,
            INextLevelHelper nextLevelHelper,
            bool hasInternetonnection = false)
        {
            base.Initialise(screensSceneGod);
            Helper.AssertIsNotNull(backButton, /*buyCaptainButton, buyHeckleButton,*/ blackMarketButton, captainsContainer, bodykitsContainer, variantsContainer);
            Helper.AssertIsNotNull(captainsButton, hecklesButton, bodykitButton, variantsButton);
            _dataProvider = dataProvider;
            _prefabFactory = prefabFactory;
            _soundPlayer = soundPlayer;

            //Initialise each button with its function
            backButton.Initialise(_soundPlayer, GoHome, this);
            /*            buyCaptainButton.Initialise(_soundPlayer, PurchaseCaptainExo, this);
                        buyHeckleButton.Initialise(_soundPlayer, PurchaseHeckle, this);*/
            captainsButton.Initialise(_soundPlayer, CaptainsButton_OnClick);
            hecklesButton.Initialise(_soundPlayer, HeckesButton_OnClick);
            bodykitButton.Initialise(_soundPlayer, BodykitButton_OnClick);
            variantsButton.Initialise(_soundPlayer, VariantsButton_OnClick);
            infoButton.Initialise(_soundPlayer, InfoButton_OnClick);
            captainsContainer.Initialize(_soundPlayer, _dataProvider, _prefabFactory);
            hecklesContainer.Initialize(_soundPlayer, _dataProvider, _prefabFactory);
            bodykitsContainer.Initialize(_soundPlayer, _dataProvider, _prefabFactory);
            variantsContainer.Initialize(_soundPlayer, _dataProvider, _prefabFactory);
            variantsContainer.itemDetailsPanel.SetActive(false);
            bodykitsContainer.itemDetailsPanel.SetActive(false);
            captainsContainer.itemDetailsPanel.SetActive(false);
            // this keeps the details panels from popping in for a frame on changing shop tabs
            commonStrings = LandingSceneGod.Instance.commonStrings;
            HighlightCaptainsNavButton();

            InternetConnection = hasInternetonnection;
            if (UnityServices.State != ServicesInitializationState.Uninitialized && InternetConnection)
            {
                // Only make cash shop available if there's an internet connection
                // Without one, we can't process transactions.
                blackMarketButton.Initialise(_soundPlayer, GotoBlackMarket, this);
                blackMarketText.text = LandingSceneGod.Instance.screenSceneStrings.GetString("BlackMarketOpen");
            }
            else
                blackMarketButton.gameObject.SetActive(false);

            exoBaseList = GeneratePseudoRandomList(14, _dataProvider.GameModel.Captains.Count - 1, 1, 1);
#if UNITY_EDITOR
            exoBaseList = GenerateFullList(_dataProvider.GameModel.Captains.Count);
#endif
            foreach (int index in exoBaseList)
            {
                CaptainExo captainExo = _prefabFactory.GetCaptainExo(StaticPrefabKeys.CaptainExos.GetCaptainExoKey(index));
                captains.Add(captainExo);
            }

            variantList = VariantsForOwnedItems();
            foreach (int index in variantList)
            {
                VariantPrefab variant = _prefabFactory.GetVariant(StaticPrefabKeys.Variants.GetVariantKey(index));
                variants.Add(variant);
            }

            bodykitList = GeneratePseudoRandomList(6, _dataProvider.GameModel.Bodykits.Count - 1, 6, 1);
#if UNITY_EDITOR
            bodykitList = GenerateFullList(_dataProvider.GameModel.Bodykits.Count);
#endif
            foreach (int index in bodykitList)
            {
                Bodykit bodykit = _prefabFactory.GetBodykit(StaticPrefabKeys.BodyKits.GetBodykitKey(index));
                bodykits.Add(bodykit);
            }
        }

        void OnEnable()
        {
            HighlightCaptainsNavButton();
        }

        //All the button fucntions for shop screen
        public void GoHome()
        {
            _screensSceneGod.GotoHubScreen();
        }

        public void InfoButton_OnClick()
        {
            ILocTable screensSceneStrings = LandingSceneGod.Instance.screenSceneStrings;
            ScreensSceneGod.Instance.messageBoxBig.ShowMessage(screensSceneStrings.GetString("ShopInfoTitle"), screensSceneStrings.GetString("ShopInfoText"));
        }

        public void GotoBlackMarket()
        {
            _screensSceneGod.GotoBlackMarketScreen();
        }

        public void CaptainsButton_OnClick()
        {
            InitiaiseCaptains();
            HighlightCaptainsNavButton();
        }

        public void HeckesButton_OnClick()
        {
            InitialiseHeckles();
            HighlightHecklesNavButton();
        }

        public void BodykitButton_OnClick()
        {
            InitialiseBodykits();
            HightlightBodykitsNavButton();
        }
        public void VariantsButton_OnClick()
        {
            InitialiseVariants();
            HightlightVariantsNavButton();
        }

        private void HightlightVariantsNavButton()
        {
            captainsButtonImage.color = navButtonInactive;
            hecklesButtonImage.color = navButtonInactive;
            bodyKitButtonImage.color = navButtonInactive;
            variantButtonImage.color = navButtonActive;
        }
        private void HightlightBodykitsNavButton()
        {
            captainsButtonImage.color = navButtonInactive;
            hecklesButtonImage.color = navButtonInactive;
            bodyKitButtonImage.color = navButtonActive;
            variantButtonImage.color = navButtonInactive;
        }
        private void HighlightCaptainsNavButton()
        {
            captainsButtonImage.color = navButtonActive;
            hecklesButtonImage.color = navButtonInactive;
            bodyKitButtonImage.color = navButtonInactive;
            variantButtonImage.color = navButtonInactive;
        }

        private void HighlightHecklesNavButton()
        {
            captainsButtonImage.color = navButtonInactive;
            hecklesButtonImage.color = navButtonActive;
            bodyKitButtonImage.color = navButtonInactive;
            variantButtonImage.color = navButtonInactive;
        }

        private void RemoveAllCaptainsFromRenderCamera()
        {
            foreach (GameObject obj in captainsContainer.visualOfCaptains)
                if (obj != null)
                    DestroyImmediate(obj);

            captainsContainer.visualOfCaptains.Clear();
        }
        public void InitialiseVariants()
        {
            captainsContainer.gameObject.SetActive(false);
            hecklesContainer.gameObject.SetActive(false);
            hecklesMessage.gameObject.SetActive(false);
            bodykitsContainer.gameObject.SetActive(false);
            variantsContainer.gameObject.SetActive(true);

            VariantItemController[] items = variantsItemContainer.gameObject.GetComponentsInChildren<VariantItemController>();
            foreach (VariantItemController item in items)
                DestroyImmediate(item.gameObject);

            variantsContainer.btnBuy.SetActive(false);
            variantsContainer.ownFeedback.SetActive(false);

            StopAllCoroutines();
            IEnumerator initialiseVariantsItemPanel = InitialiseVariantsItemPanel();
            StartCoroutine(initialiseVariantsItemPanel);
            // Show the message panel and hide the item details initially
            variantsContainer.variantMessagePanel.SetActive(true);
            variantsContainer.itemDetailsPanel.SetActive(false);
            bodykitsContainer.itemDetailsPanel.SetActive(false);
            captainsContainer.itemDetailsPanel.SetActive(false);
            // this keeps the details panels from popping in for a frame on changing shop tabs

            variantsContainer.t_variantsMessage.text = LandingSceneGod.Instance.screenSceneStrings.GetString("VariantsShopHelp");
        }

        public void InitialiseBodykits()
        {
            captainsContainer.gameObject.SetActive(false);
            hecklesContainer.gameObject.SetActive(false);
            hecklesMessage.gameObject.SetActive(false);
            bodykitsContainer.gameObject.SetActive(true);
            variantsContainer.gameObject.SetActive(false);

            BodykitItemController[] items = bodykitItemContainer.gameObject.GetComponentsInChildren<BodykitItemController>();
            foreach (BodykitItemController item in items)
                DestroyImmediate(item.gameObject);

            bodykitsContainer.btnBuy.SetActive(false);
            bodykitsContainer.ownFeedback.SetActive(false);

            //    List<int> bodykitList = GeneratePseudoRandomList(6, 11, 6, 1);

            byte ii = 0;
            // bodykitItemContainer.gameObject.SetActive(false);
            foreach (int index in bodykitList)
            {
                GameObject bodykitItem = Instantiate(bodykitItemPrefab, bodykitItemContainer);
                Bodykit bodykit = bodykits[ii]/*await _prefabFactory.GetBodykit(StaticPrefabKeys.BodyKits.AllKeys[index])*/;
                bodykitItem.GetComponent<BodykitItemController>().StaticInitialise(_soundPlayer, bodykit.bodykitImage, _dataProvider.GameModel.Bodykits[index], bodykitsContainer, _dataProvider, _prefabFactory, ii);
                if (ii == 0)
                {
                    bodykitItem.GetComponent<BodykitItemController>()._clickedFeedback.SetActive(true);
                    bodykitsContainer.currentItem = bodykitItem.GetComponent<BodykitItemController>();
                    bodykitsContainer.bodykitImage.sprite = bodykit.bodykitImage;
                    bodykitsContainer.bodykitPrice.text = _dataProvider.GameModel.Bodykits[index].bodykitCost.ToString();
                    bodykitsContainer.bodykitName.text = commonStrings.GetString(_dataProvider.GameModel.Bodykits[index].nameStringKeyBase);
                    bodykitsContainer.bodykitDescription.text = commonStrings.GetString(_dataProvider.GameModel.Bodykits[index].descriptionKeyBase);
                    bodykitsContainer.currentBodykitData = _dataProvider.GameModel.Bodykits[index];
                    if (_dataProvider.GameModel.Bodykits[index].isOwned)
                    {
                        bodykitsContainer.btnBuy.SetActive(false);
                        bodykitsContainer.ownFeedback.SetActive(true);
                    }
                    else
                    {
                        bodykitsContainer.btnBuy.SetActive(true);
                        bodykitsContainer.ownFeedback.SetActive(false);
                    }
                }
                ii++;
            }
            // Show the message panel and hide the item details initially
            bodykitsContainer.bodykitMessagePanel.SetActive(true);
            variantsContainer.itemDetailsPanel.SetActive(false);
            bodykitsContainer.itemDetailsPanel.SetActive(false);
            captainsContainer.itemDetailsPanel.SetActive(false);
            // this keeps the details panels from popping in for a frame on changing shop tabs

            bodykitsContainer.t_bodykitsMessage.text = LandingSceneGod.Instance.screenSceneStrings.GetString("BodykitsShopHelp");
        }
        public void InitialiseHeckles()
        {
            captainsContainer.gameObject.SetActive(false);
            hecklesContainer.gameObject.SetActive(true);
            bodykitsContainer.gameObject.SetActive(false);
            variantsContainer.gameObject.SetActive(false);

            hecklesMessage.gameObject.SetActive(true);
            // remove all old children to refresh
            HeckleItemController[] items = heckleItemContainer.gameObject.GetComponentsInChildren<HeckleItemController>();
            foreach (HeckleItemController item in items)
                DestroyImmediate(item.gameObject);

            RemoveAllCaptainsFromRenderCamera();


            hecklesContainer.btnBuy.SetActive(false);
            hecklesContainer.ownFeedback.SetActive(false);
            CaptainExo charliePrefab = _prefabFactory.GetCaptainExo(_dataProvider.GameModel.PlayerLoadout.CurrentCaptain);
            CaptainExo captainExo = Instantiate(charliePrefab, captainCamContainer);
            captainExo.gameObject.transform.localScale = Vector3.one * 0.5f;
            captainsContainer.visualOfCaptains.Add(captainExo.gameObject);

            List<int> heckleBaseList = GeneratePseudoRandomList(15, _dataProvider.GameModel.Heckles.Count - 1, 10);
#if UNITY_EDITOR
            heckleBaseList = GenerateFullList(_dataProvider.GameModel.Heckles.Count);
#endif
            byte ii = 0;
            foreach (int index in heckleBaseList)
            {
                GameObject heckleItem = Instantiate(heckleItemPrefab, heckleItemContainer) as GameObject;
                heckleItem.GetComponent<HeckleItemController>().StaticInitialise(_soundPlayer, _dataProvider.GameModel.Heckles[index], hecklesContainer, ii);
                if (ii == 0)
                {
                    heckleItem.GetComponent<HeckleItemController>()._clickedFeedback.SetActive(true);
                    hecklesContainer.currentItem = heckleItem.GetComponent<HeckleItemController>();

                    heckleItem.GetComponent<HeckleItemController>().OnClicked();
                    hecklesContainer.hecklePrice.text = _dataProvider.GameModel.Heckles[index].heckleCost.ToString();
                    hecklesContainer.currentHeckleData = _dataProvider.GameModel.Heckles[index];
                    //hecklesContainer.t_heckleMessage.text = LandingSceneGod.Instance.hecklesStrings.GetString(_dataProvider.GameModel.Heckles[index].StringKeyBase);

                    if (_dataProvider.GameModel.Heckles[index].IsOwned)
                    {
                        hecklesContainer.hecklePrice.text = "0";
                        hecklesContainer.btnBuy.SetActive(false);
                        hecklesContainer.ownFeedback.SetActive(true);
                    }
                    else
                    {
                        hecklesContainer.btnBuy.SetActive(true);
                        hecklesContainer.ownFeedback.SetActive(false);
                    }
                }
                ii++;
            }

            // special heckle-specific infotext handling here
            hecklesContainer.t_heckleMessage.text = LandingSceneGod.Instance.screenSceneStrings.GetString("HecklesShopHelp");
        }

        public override void OnDismissing()
        {
            base.OnDismissing();
            RemoveAllCaptainsFromRenderCamera();
        }
        public void InitiaiseCaptains()
        {
            captainsContainer.gameObject.SetActive(true);
            hecklesContainer.gameObject.SetActive(false);
            bodykitsContainer.gameObject.SetActive(false);
            variantsContainer.gameObject.SetActive(false);

            hecklesContainer.gameObject.SetActive(false);
            hecklesMessage.gameObject.SetActive(false);
            // remove all old children to refersh
            CaptainItemController[] items = captainItemContainer.gameObject.GetComponentsInChildren<CaptainItemController>();
            foreach (CaptainItemController item in items)
                DestroyImmediate(item.gameObject);

            captainsContainer.btnBuy.SetActive(false);
            captainsContainer.ownFeedback.SetActive(false);


            RemoveAllCaptainsFromRenderCamera();

            /*            exoBaseList = GeneratePseudoRandomList(14, _dataProvider.GameModel.Captains.Count - 1, 1, 1);
                        exoBaseList.Insert(0, 0);*/

            byte ii = 0;
            foreach (int index in exoBaseList)
            {
                GameObject captainItem = Instantiate(captainItemPrefab, captainItemContainer) as GameObject;
                CaptainExo captainExoPrefab = captains[ii];/*await _prefabFactory.GetCaptainExo(StaticPrefabKeys.CaptainExos.GetCaptainExoKey(index));*/
                CaptainExo captainExo = Instantiate(captainExoPrefab, captainCamContainer);
                captainExo.gameObject.transform.localScale = Vector3.one * 0.5f;
                captainExo.gameObject.SetActive(false);
                captainsContainer.visualOfCaptains.Add(captainExo.gameObject);
                captainItem.GetComponent<CaptainItemController>().StaticInitialise(_soundPlayer, captainExo.CaptainExoImage, _dataProvider.GameModel.Captains[index], captainsContainer, ii);

                if (ii == 0)  // the first item should be clicked :)
                {
                    captainItem.GetComponent<CaptainItemController>()._clickedFeedback.SetActive(true);
                    captainsContainer.currentItem = captainItem.GetComponent<CaptainItemController>();
                    //    captainItem.GetComponent<CaptainItemController>().OnClicked(); // to display Captain's price
                    if (index == 0)
                    {
                        captainsContainer.captainPrice.text = "0"; // CaptainExo000 is default item. :)
                        captainsContainer.captainName.text = commonStrings.GetString(_dataProvider.GameModel.Captains[0].nameStringKeyBase);
                        captainsContainer.captainDescription.text = commonStrings.GetString(_dataProvider.GameModel.Captains[0].descriptionKeyBase);
                    }
                    captainExo.gameObject.SetActive(true);
                    if (_dataProvider.GameModel.Captains[index].IsOwned)
                    {
                        captainsContainer.btnBuy.SetActive(false);
                        captainsContainer.ownFeedback.SetActive(true);
                    }
                    else
                    {
                        captainsContainer.btnBuy.SetActive(true);
                        captainsContainer.ownFeedback.SetActive(false);
                    }
                }
                ii++;
            }

            captainsContainer.captainMessagePanel.SetActive(true);
            variantsContainer.itemDetailsPanel.SetActive(false);
            bodykitsContainer.itemDetailsPanel.SetActive(false);
            captainsContainer.itemDetailsPanel.SetActive(false);
            // this keeps the details panels from popping in for a frame on changing shop tabs
            captainsContainer.t_captainMessage.text = LandingSceneGod.Instance.screenSceneStrings.GetString("CaptainsShopHelp");
        }

        List<int> GeneratePseudoRandomList(int elements, int maxValue, int dailyShift, int startValue = 0)  //elements = number of elements in output list
        {
            DateTime utcNow = DateTime.UtcNow;
            List<int> randomList = new List<int>();
            for (int i = startValue; i < elements + startValue; i++)
                randomList.Add(startValue + (maxValue / elements * i + dailyShift * utcNow.Day + utcNow.Month) % (1 + maxValue - startValue));

            return randomList;
        }

        List<int> VariantsForOwnedItems()
        {
            List<int> variantsList = new List<int>();
            IList<BuildingKey> buildingKeys = _dataProvider.GameModel.PlayerLoadout.GetAllBuildings();
            IList<UnitKey> unitKeys = _dataProvider.GameModel.PlayerLoadout.GetAllUnits();
            List<string> buildablePrefabNames = new List<string>();

            for (int i = 0; i < buildingKeys.Count; i++)
                buildablePrefabNames.Add(buildingKeys[i].PrefabName);

            for (int i = 0; i < unitKeys.Count; i++)
                buildablePrefabNames.Add(unitKeys[i].PrefabName);


            for (int i = 0; i < _dataProvider.GameModel.Variants.Count; i++)
            {
                VariantPrefab variant = _prefabFactory.GetVariant(StaticPrefabKeys.Variants.GetVariantKey(i));

                for (int j = 0; j < buildablePrefabNames.Count; j++)
                    if (variant.parent.ToString() == buildablePrefabNames[j])
                        variantsList.Add(variant.variantIndex);
            }

            return variantsList;
        }

        List<int> GenerateFullList(int elements)
        {
            List<int> fullList = new List<int>();
            for (int i = 0; i < elements; i++)
                fullList.Add(i);

            return fullList;
        }

        IEnumerator InitialiseVariantsItemPanel()
        {
            int completedVariants = 0;
            int variantsPerFrame = 15;
            byte ii = 0;
            while (completedVariants < variantList.Count)
            {
                for (int i = completedVariants; i < Mathf.Min(completedVariants + variantsPerFrame, variantList.Count); i++)
                {
                    GameObject variantItem = Instantiate(variantItemPrefab, variantsItemContainer) as GameObject;
                    VariantPrefab variant = variants[ii];/*await _prefabFactory.GetVariant(StaticPrefabKeys.Variants.AllKeys[index]);*/
                    Sprite parentSprite = variant.IsUnit() ? variant.GetUnit(ScreensSceneGod.Instance._prefabFactory).Sprite : variant.GetBuilding(ScreensSceneGod.Instance._prefabFactory).Sprite;
                    variantItem.GetComponent<VariantItemController>().StaticInitialise(_soundPlayer, parentSprite, variant.variantSprite, variant.GetParentName(ScreensSceneGod.Instance._prefabFactory), _dataProvider.GameModel.Variants[variant.variantIndex], variantsContainer, variant, variant.variantIndex);
                    if (ii == 0)
                    {
                        variantItem.GetComponent<VariantItemController>()._clickedFeedback.SetActive(true);
                        variantItem.GetComponent<VariantItemController>()._clickedFeedbackVariantImage.color = new Color(variantItem.GetComponent<VariantItemController>()._clickedFeedbackVariantImage.color.r, variantItem.GetComponent<VariantItemController>()._clickedFeedbackVariantImage.color.g, variantItem.GetComponent<VariantItemController>()._clickedFeedbackVariantImage.color.b, 1f);
                        variantsContainer.currentItem = variantItem.GetComponent<VariantItemController>();
                        variantsContainer.ParentImage.sprite = parentSprite;
                        variantsContainer.VariantPrice.text = _dataProvider.GameModel.Variants[variant.variantIndex].variantCredits.ToString();
                        variantsContainer.variantIcon.sprite = variant.variantSprite;
                        variantsContainer.VariantName.text = commonStrings.GetString(_dataProvider.GameModel.Variants[variant.variantIndex].variantNameStringKeyBase);
                        variantsContainer.variantDescription.text = commonStrings.GetString(_dataProvider.GameModel.Variants[variant.variantIndex].variantDescriptionStringKeyBase);
                        variantsContainer.ParentName.text = variant.GetParentName(ScreensSceneGod.Instance._prefabFactory);
                        variantsContainer.currentVariantData = _dataProvider.GameModel.Variants[variant.variantIndex];

                        if (variant.IsUnit())
                        {
                            variantsContainer.buildingStatsController.gameObject.SetActive(false);
                            variantsContainer.unitStatsController.gameObject.SetActive(true);
                            variantsContainer.unitStatsController.ShowStatsOfVariant(variant.GetUnit(ScreensSceneGod.Instance._prefabFactory), variant);
                        }
                        else
                        {
                            variantsContainer.buildingStatsController.gameObject.SetActive(true);
                            variantsContainer.unitStatsController.gameObject.SetActive(false);
                            variantsContainer.buildingStatsController.ShowStatsOfVariant(variant.GetBuilding(ScreensSceneGod.Instance._prefabFactory), variant);
                        }

                        if (_dataProvider.GameModel.Variants[variant.variantIndex].isOwned)
                        {
                            variantsContainer.btnBuy.SetActive(false);
                            variantsContainer.ownFeedback.SetActive(true);
                        }
                        else
                        {
                            variantsContainer.btnBuy.SetActive(true);
                            variantsContainer.ownFeedback.SetActive(false);
                        }
                    }
                    ii++;
                }
                completedVariants += Mathf.Min(variantsPerFrame, variantList.Count);
                yield return 0;
            }
        }
    }
}