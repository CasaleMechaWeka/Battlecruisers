using BattleCruisers.Buildables.Buildings;
using BattleCruisers.Buildables.Units;
using BattleCruisers.Data;
using BattleCruisers.Scenes;
using BattleCruisers.UI.Common.BuildableDetails.Stats;
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
        private IVariantData currentVariantData;
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

        }
    }

    public class VariantDataEventArgs : EventArgs
    {

    }
}

