using BattleCruisers.Buildables.Buildings;
using BattleCruisers.Buildables.Units;
using BattleCruisers.Data;
using BattleCruisers.Scenes;
using BattleCruisers.UI.Common.BuildableDetails.Stats;
using BattleCruisers.UI.ScreensScene.BattleHubScreen;
using BattleCruisers.UI.ScreensScene.ShopScreen;
using BattleCruisers.UI.Sound.Players;
using BattleCruisers.Utils.Fetchers;
using BattleCruisers.Utils.Localisation;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace BattleCruisers.UI.ScreensScene
{
    public class VariantsContainer : MonoBehaviour
    {
        public Image ParentImage;
        public Text ParentName;
        public Text VariantName;
        public Image variantIcon;
        public Text variantDescription;
        public Text VariantPrice;
        public StatsController<IBuilding> buildingStatsController;
        public StatsController<IUnit> unitStatsController;

        public EventHandler<VariantDataEventArgs> variantDataChanged;
        public ILocTable commonStrings;
        private ILocTable screensSceneTable;
        public VariantItemController currentItem;
        public IVariantData currentVariantData;
        public GameObject btnBuy, ownFeedback;

        private ISingleSoundPlayer _soundPlayer;
        private IDataProvider _dataProvider;
        private IPrefabFactory _prefabFactory;
        public GameObject content;


        public void Initialize(ISingleSoundPlayer soundPlayer, IDataProvider dataProvider, IPrefabFactory prefabFactory)
        {
            commonStrings = LandingSceneGod.Instance.commonStrings;
            screensSceneTable = LandingSceneGod.Instance.screenSceneStrings;
            variantDataChanged += VariantDataChanged;
            _soundPlayer = soundPlayer;
            _dataProvider = dataProvider;
            _prefabFactory = prefabFactory;
            btnBuy.GetComponent<CanvasGroupButton>().Initialise(_soundPlayer, Purchase);
        }

        private async void Purchase()
        {

        }

        private void VariantDataChanged(object sender, VariantDataEventArgs e)
        {
            currentItem._clickedFeedback.SetActive(false);
            currentItem = (VariantItemController)sender;
            currentVariantData = e.variantData;
            ScreensSceneGod.Instance.characterOfShop.GetComponent<Animator>().SetTrigger("select");

            if(e.variantData.IsOwned)
            {
                btnBuy.SetActive(false);
                ownFeedback.SetActive(true);
            }
            else
            {
                btnBuy.SetActive(true);
                ownFeedback.SetActive(false);
            }

            VariantPrice.text = e.variantData.VariantCost.ToString();
            ParentImage.sprite = e.parentSprite;
            variantIcon.sprite = e.variantSprite;
            VariantName.text = commonStrings.GetString(e.variantData.VariantNameStringKeyBase);
            variantDescription.text = commonStrings.GetString(e.variantData.VariantDescriptionStringKeyBase);
            ParentName.text = e.parentName;
        }

        private void OnDestroy()
        {
            variantDataChanged -= VariantDataChanged;
        }
        public void GotoBlackMarket()
        {
            GetComponentInParent<ShopPanelScreenController>().GotoBlackMarket();
        }
    }

    public class VariantDataEventArgs : EventArgs
    {
        public IVariantData variantData { get; set; }
        public Sprite parentSprite { get; set; }
        public Sprite variantSprite { get; set; }
        public string parentName { get; set; }
    }
}

