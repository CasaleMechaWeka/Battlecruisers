using BattleCruisers.Buildables;
using BattleCruisers.Data.Static;
using BattleCruisers.UI.BattleScene.Presentables;
using BattleCruisers.UI.Filters;
using BattleCruisers.UI.Sound;
using BattleCruisers.UI.Sound.Players;
using BattleCruisers.Utils;
using System;
using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.UI;

namespace BattleCruisers.UI.BattleScene.Buttons
{
    public abstract class BuildableButtonController : ClickablePresentableController, IBuildableButton, IBroadcastingFilter
	{
        private IBroadcastingFilter<IBuildable> _shouldBeEnabledFilter;
        private FilterToggler _isEnabledToggler;

		public Image buildableImage;
        
        public Image redGlowImage;

        public Image buildableImageOutline;//modified
		public Text buildableName;
		public Text droneLevel;

        public Color redColor;

        public Color redGlowColor;

        public event EventHandler PotentialMatchChange;

        public IBuildable Buildable { get; private set; }

        private CanvasGroup _canvasGroup;
        protected override CanvasGroup CanvasGroup => _canvasGroup;

        protected override bool Disable => false;
        protected override ISoundKey ClickSound => SoundKeys.UI.Click;

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
                    redGlowImage.color = redGlowColor;
                }
                else{
                    buildableImageOutline.color = Color.clear;
                    buildableName.color = redColor;
                    redGlowImage.color = Color.clear;
                }//holy shit i am insane
                
            }
        }

        public void Initialise(ISingleSoundPlayer soundPlayer, IBuildable buildable, IBroadcastingFilter<IBuildable> shouldBeEnabledFilter)
		{
			base.Initialise(soundPlayer);

            Helper.AssertIsNotNull(buildableImage, buildableName, droneLevel);
            Helper.AssertIsNotNull(buildable, shouldBeEnabledFilter);

			Buildable = buildable;

            _shouldBeEnabledFilter = shouldBeEnabledFilter;
            _shouldBeEnabledFilter.PotentialMatchChange += _shouldBeEnabledFilter_PotentialMatchChange;

            buildableName.text = Buildable.Name;
            droneLevel.text = Buildable.NumOfDronesRequired.ToString();
            buildableImage.sprite = Buildable.Sprite;

            _canvasGroup = GetComponent<CanvasGroup>();
            Assert.IsNotNull(_canvasGroup);

            _isEnabledToggler = new FilterToggler(this, this);
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
    }
}
