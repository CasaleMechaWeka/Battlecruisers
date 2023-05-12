using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Buildables;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Data.Static;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.UI.BattleScene.Presentables;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.UI.Filters;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.UI.Sound;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.UI.Sound.Players;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Utils;
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

        public Image redGlowImage;

        public Image buildableImageOutline;//modified
        public Text buildableName;
        public Text droneLevel;

        public Color redColor;

        public Color redGlowColor;

        public event EventHandler PotentialMatchChange;

        public IPvPBuildable Buildable { get; private set; }

        private CanvasGroup _canvasGroup;
        protected override CanvasGroup CanvasGroup => _canvasGroup;

        protected override bool Disable => false;
        protected override IPvPSoundKey ClickSound => PvPSoundKeys.UI.Click;

        public virtual bool IsMatch => _shouldBeEnabledFilter.IsMatch(Buildable);
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
                    isSelected = true;
                }
                else
                {
                    buildableImageOutline.color = Color.clear;
                    buildableName.color = redColor;
                    upgradeIconImage1.color = redColor;
                    upgradeIconImage2.color = redColor;
                    upgradeIconImage3.color = redColor;
                    upgradeIconImage4.color = redColor;
                    upgradeIconImage5.color = redColor;
                    warheadIconImage.color = redColor;
                    redGlowImage.color = Color.clear;
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
