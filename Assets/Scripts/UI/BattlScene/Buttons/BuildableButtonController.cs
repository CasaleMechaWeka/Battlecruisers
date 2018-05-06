using System;
using BattleCruisers.Buildables;
using BattleCruisers.UI.BattleScene.Manager;
using BattleCruisers.Utils;
using UnityEngine.UI;

namespace BattleCruisers.UI.BattleScene.Buttons
{
    public abstract class BuildableButtonController : Presentable, IBuildableButton, IActivenessDecider
	{
		protected IUIManager _uiManager;
        private IActivenessDecider<IBuildable> _activenessDecider;

		public Image buildableImage;
		public Text buildableName;
		public Text droneLevel;

        public event EventHandler Clicked;
        public event EventHandler PotentialActivenessChange;

        public IBuildable Buildable { get; private set; }

        public virtual bool ShouldBeEnabled { get { return _activenessDecider.ShouldBeEnabled(Buildable); } }

        public void Initialise(IBuildable buildable, IUIManager uiManager, IActivenessDecider<IBuildable> activenessDecider)
		{
			base.Initialise();

            Helper.AssertIsNotNull(buildable, uiManager, activenessDecider);

			Buildable = buildable;
			_uiManager = uiManager;

            _activenessDecider = activenessDecider;
            _activenessDecider.PotentialActivenessChange += _activenessDecider_PotentialActivenessChange;

            buildableName.text = Buildable.Name;
            droneLevel.text = Buildable.NumOfDronesRequired.ToString();
            buildableImage.sprite = Buildable.Sprite;

            ButtonWrapper buttonWrapper = GetComponent<ButtonWrapper>();
            buttonWrapper.Initialise(HandleClick, this);
		}

        private void _activenessDecider_PotentialActivenessChange(object sender, EventArgs e)
        {
            TriggerActivenessChange();
        }

        protected void TriggerActivenessChange()
        {
            if (PotentialActivenessChange != null)
            {
                PotentialActivenessChange.Invoke(this, EventArgs.Empty);
            }
        }

		protected virtual void HandleClick()
        {
            if (Clicked != null)
            {
                Clicked.Invoke(this, EventArgs.Empty);
            }
        }
	}
}
