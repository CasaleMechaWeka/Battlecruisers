using BattleCruisers.Buildables;
using BattleCruisers.UI.BattleScene.Presentables;
using BattleCruisers.UI.Filters;
using BattleCruisers.Utils;
using System;
using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace BattleCruisers.UI.BattleScene.Buttons
{
    public abstract class BuildableButtonController : PresentableController, 
        IBuildableButton, 
        IBroadcastingFilter,
        IPointerClickHandler
	{
        private IBroadcastingFilter<IBuildable> _shouldBeEnabledFilter;
        private FilterToggler _isEnabledToggler;

		public Image buildableImage;
		public Text buildableName;
		public Text droneLevel;

        public event EventHandler Clicked;
        public event EventHandler PotentialMatchChange;

        public IBuildable Buildable { get; private set; }

        private CanvasGroup _canvasGroup;
        protected override CanvasGroup CanvasGroup => _canvasGroup;

        protected override bool Disable => false;

        public virtual bool IsMatch => _shouldBeEnabledFilter.IsMatch(Buildable);
        public Color Color
        {
            set
            {
                buildableImage.color = value;
                buildableName.color = value;
                droneLevel.color = value;
            }
        }

        public void Initialise(IBuildable buildable, IBroadcastingFilter<IBuildable> shouldBeEnabledFilter)
		{
			base.Initialise();

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

        protected abstract void OnClicked(bool isButtonEnabled);

        public void OnPointerClick(PointerEventData eventData)
        {
            OnClicked(IsMatch);

            Clicked?.Invoke(this, EventArgs.Empty);
        }
    }
}
