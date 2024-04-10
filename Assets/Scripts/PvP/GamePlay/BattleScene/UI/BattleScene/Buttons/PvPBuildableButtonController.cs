using BattleCruisers.Data;
using BattleCruisers.Data.Static;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Buildables;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Buildables.Buildings;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Buildables.Units;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Data.Static;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.UI.BattleScene.Presentables;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.UI.Filters;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.UI.Sound;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.UI.Sound.Players;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Utils;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Utils.Fetchers;
using BattleCruisers.UI.ScreensScene.ProfileScreen;
using BattleCruisers.Utils.Localisation;
using System;
using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.UI.BattleScene.Buttons
{
    public abstract class PvPBuildableButtonController : PvPClickablePresentableController, IPvPBuildableButton, IPvPBroadcastingFilter
    {
        private IPvPBroadcastingFilter<IPvPBuildable> _shouldBeEnabledFilter;
        private PvPFilterToggler _isEnabledToggler;

        public Image buildableImage;

        public Image upgradeIconImage1;
        public Image upgradeIconImage2;
        public Image upgradeIconImage3;
        public Image upgradeIconImage4;
        public Image upgradeIconImage5;
        public Image warheadIconImage;

        public GameObject upgradeIconImage1Object;
        public GameObject upgradeIconImage2Object;
        public GameObject upgradeIconImage3Object;
        public GameObject upgradeIconImage4Object;
        public GameObject upgradeIconImage5Object;
        public GameObject warheadIconImageObject;

        public Image redGlowImage;



        public Image buildableImageOutline;//modified

        public Image buildableButton;

        public Sprite originalOutlineSprite;
        public Sprite originalButtonSprite;

        public Sprite variantOutlineSprite;
        public Sprite variantButtonSprite;

        public Text buildableName;
        public Text droneLevel;
        public Image droneIcon;

        public Color redColor;

        public Color redGlowColor;

        public event EventHandler PotentialMatchChange;

        public IPvPBuildable Buildable { get; private set; }

        private CanvasGroup _canvasGroup;
        protected override CanvasGroup CanvasGroup => _canvasGroup;

        protected override bool Disable => false;
        protected override IPvPSoundKey ClickSound => PvPSoundKeys.UI.Click;

        public virtual bool IsMatch => current_variant == null ? _shouldBeEnabledFilter.IsMatch(Buildable) : _shouldBeEnabledFilter.IsMatch(Buildable, current_variant);

        private VariantPrefab current_variant = null;
        public Color Color
        {
            set
            {
                //buildableImage.color = value;
                //modified
                //droneLevel.color = value;
                if (value.Equals(Color.white))
                {
                    buildableImageOutline.color = value;//modified
                    buildableName.color = value;
                    upgradeIconImage1.color = value;
                    upgradeIconImage2.color = value;
                    upgradeIconImage3.color = value;
                    upgradeIconImage4.color = value;
                    upgradeIconImage5.color = value;
                    warheadIconImage.color = value;
                    redGlowImage.color = redGlowColor;
                    droneLevel.color = Color.black; // Or any original color
                    droneIcon.color = Color.black; // Assuming black is the original color
                    isSelected = true;
                }
                else
                {
                    buildableImageOutline.color = Color.clear;
                    buildableName.color = redColor;
                    upgradeIconImage1.color = Color.black;
                    upgradeIconImage2.color = Color.black;
                    upgradeIconImage3.color = Color.black;
                    upgradeIconImage4.color = Color.black;
                    upgradeIconImage5.color = Color.black;
                    warheadIconImage.color = Color.black;
                    redGlowImage.color = Color.clear;
                    droneLevel.color = Color.black;
                    droneIcon.color = Color.black;
                    isSelected = false;
                }

            }
        }

        public bool isSelected = false;
        public void Initialise(IPvPSingleSoundPlayer soundPlayer, IPvPBuildable buildable, IPvPBroadcastingFilter<IPvPBuildable> shouldBeEnabledFilter)
        {
            base.Initialise(soundPlayer);

            PvPHelper.AssertIsNotNull(buildableImage, buildableName, droneLevel);
            PvPHelper.AssertIsNotNull(buildable, shouldBeEnabledFilter);

            Buildable = buildable;

            _shouldBeEnabledFilter = shouldBeEnabledFilter;
            _shouldBeEnabledFilter.PotentialMatchChange += _shouldBeEnabledFilter_PotentialMatchChange;

            buildableName.text = Buildable.Name;
            droneLevel.text = Buildable.NumOfDronesRequired.ToString();
            buildableImage.sprite = Buildable.Sprite;

            _canvasGroup = GetComponent<CanvasGroup>();
            Assert.IsNotNull(_canvasGroup);

            _isEnabledToggler = new PvPFilterToggler(this, this);
        }

        public async void ApplyVariantIfExist(IPvPBuilding building)
        {
            IDataProvider dataProvder = ApplicationModelProvider.ApplicationModel.DataProvider;
            IPvPPrefabFactory prefabFactory = PvPBattleSceneGodClient.Instance.factoryProvider.PrefabFactory;

            ILocTable commonString = await LocTableFactory.Instance.LoadCommonTableAsync();
            int index = await dataProvder.GameModel.PlayerLoadout.GetSelectedBuildingVariantIndex(prefabFactory, building);
            if (index != -1)
            {
                VariantPrefab variant = await prefabFactory.GetVariant(StaticPrefabKeys.Variants.GetVariantKey(index));
                if (variant != null)
                {
                    current_variant = variant;
                    //buildableName.text = commonString.GetString(dataProvder.GameModel.Variants[index].VariantNameStringKeyBase);
                    droneLevel.text = (building.NumOfDronesRequired + variant.statVariant.drone_num).ToString();
                    upgradeIconImage1Object.SetActive(true);
                    upgradeIconImage1.sprite = variant.variantSprite;

                    // Swap sprites for variant
                    buildableImageOutline.sprite = variantOutlineSprite;
                    buildableButton.sprite = variantButtonSprite;
                }
            }
            else
            {
                upgradeIconImage1Object.SetActive(false);
                // Reset to original sprites if not a variant
                buildableImageOutline.sprite = originalOutlineSprite;
                buildableButton.sprite = originalButtonSprite;
            }
        }


        public async void ApplyVariantIfExist(IPvPUnit unit)
        {
            IDataProvider dataProvder = ApplicationModelProvider.ApplicationModel.DataProvider;
            IPvPPrefabFactory prefabFactory = PvPBattleSceneGodClient.Instance.factoryProvider.PrefabFactory;
            ILocTable commonString = await LocTableFactory.Instance.LoadCommonTableAsync();
            int index = await dataProvder.GameModel.PlayerLoadout.GetSelectedUnitVariantIndex(prefabFactory, unit);
            if (index != -1)
            {
                VariantPrefab variant = await prefabFactory.GetVariant(StaticPrefabKeys.Variants.GetVariantKey(index));
                if (variant != null)
                {
                    current_variant = variant;
                    //buildableName.text = commonString.GetString(dataProvder.GameModel.Variants[index].VariantNameStringKeyBase);
                    droneLevel.text = (unit.NumOfDronesRequired + variant.statVariant.drone_num).ToString();
                    upgradeIconImage1Object.SetActive(true);
                    upgradeIconImage1.sprite = variant.variantSprite;

                    // Swap sprites for variant
                    buildableImageOutline.sprite = /*buildableImageOutlineUpgrade1.sprite;*/ variantOutlineSprite;
                    buildableButton.sprite = /*buildableButtonUpgrade1.sprite;*/ variantButtonSprite;
                }
            }
            else
            {
                upgradeIconImage1Object.SetActive(false);
                buildableImageOutline.sprite = originalOutlineSprite;
                buildableButton.sprite = originalButtonSprite;
            }
        }


        private void _shouldBeEnabledFilter_PotentialMatchChange(object sender, EventArgs e)
        {
            TriggerPotentialMatchChange();
        }

        protected void TriggerPotentialMatchChange()
        {
            PotentialMatchChange?.Invoke(this, EventArgs.Empty);
        }

        protected override void OnClicked()
        {
            base.OnClicked();
            HandleClick(IsMatch);
        }

        protected abstract void HandleClick(bool isButtonEnabled);

        public void TriggerClick()
        {
            OnClicked();
        }

        public override void OnPointerDown(PointerEventData eventData)
        {
            base.OnPointerDown(eventData);
        }

        public override void OnPointerUp(PointerEventData eventData)
        {
            base.OnPointerUp(eventData);
        }

        public override void OnPointerEnter(PointerEventData eventData)
        {
            base.OnPointerEnter(eventData);
            HandleHover();
        }

        public override void OnPointerExit(PointerEventData eventData)
        {
            base.OnPointerExit(eventData);
            HandleHoverExit();
        }

        public virtual void HandleHover() { }

        public virtual void HandleHoverExit() { }
    }
}
