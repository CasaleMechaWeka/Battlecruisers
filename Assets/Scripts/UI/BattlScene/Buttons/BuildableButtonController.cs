using BattleCruisers.Buildables;
using BattleCruisers.UI.BattleScene.Manager;
using BattleCruisers.UI.BattleScene.Presentables;
using BattleCruisers.UI.Filters;
using BattleCruisers.Utils;
using System;
using UnityEngine;
using UnityEngine.UI;

namespace BattleCruisers.UI.BattleScene.Buttons
{
    public abstract class BuildableButtonController : PresentableController, IBuildableButton, IBroadcastingFilter
	{
		protected IUIManager _uiManager;
        private IBroadcastingFilter<IBuildable> _shouldBeEnabledFilter;
        private ButtonWrapper _buttonWrapper;

		public Image buildableImage;
		public Text buildableName;
		public Text droneLevel;

        public event EventHandler Clicked;
        public event EventHandler PotentialMatchChange;

        public IBuildable Buildable { get; private set; }

        public virtual bool IsMatch { get { return _shouldBeEnabledFilter.IsMatch(Buildable); } }

        public void Initialise(IBuildable buildable, IUIManager uiManager, IBroadcastingFilter<IBuildable> shouldBeEnabledFilter)
		{
			base.Initialise();

            Helper.AssertIsNotNull(buildable, uiManager, shouldBeEnabledFilter);

			Buildable = buildable;
			_uiManager = uiManager;

            _shouldBeEnabledFilter = shouldBeEnabledFilter;
            _shouldBeEnabledFilter.PotentialMatchChange += _shouldBeEnabledFilter_PotentialMatchChange;

            buildableName.text = Buildable.Name;
            droneLevel.text = Buildable.NumOfDronesRequired.ToString();
            buildableImage.sprite = Buildable.Sprite;

            _buttonWrapper = GetComponent<ButtonWrapper>();
            _buttonWrapper.Initialise(HandleClick, this, disableButton: false);
		}

        private void _shouldBeEnabledFilter_PotentialMatchChange(object sender, EventArgs e)
        {
            TriggerPotentialMatchChange();
        }

        protected void TriggerPotentialMatchChange()
        {
            if (PotentialMatchChange != null)
            {
                PotentialMatchChange.Invoke(this, EventArgs.Empty);
            }
        }

		protected void HandleClick()
        {
            // FELIX  Remove
            Debug.Log("BuildableButtonController.HandleClic()");

            if (IsMatch)
            {
                OnClicked();
            }
            else
            {
                // FELIX  Play not enough drones sound :)
            }

            if (Clicked != null)
            {
                Clicked.Invoke(this, EventArgs.Empty);
            }
        }

        protected abstract void OnClicked();
	}
}
