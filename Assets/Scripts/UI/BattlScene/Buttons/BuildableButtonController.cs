using System;
using BattleCruisers.Buildables;
using BattleCruisers.UI.BattleScene.Manager;
using BattleCruisers.Utils;
using UnityEngine.UI;

namespace BattleCruisers.UI.BattleScene.Buttons
{
    public abstract class BuildableButtonController : Presentable, IBuildableButton, IFilter
	{
		protected IUIManager _uiManager;
        private IFilter<IBuildable> _activenessDecider;

		public Image buildableImage;
		public Text buildableName;
		public Text droneLevel;

        public event EventHandler Clicked;
        public event EventHandler PotentialMatchChange;

        public IBuildable Buildable { get; private set; }

        public virtual bool IsMatch { get { return _activenessDecider.IsMatch(Buildable); } }

        public void Initialise(IBuildable buildable, IUIManager uiManager, IFilter<IBuildable> activenessDecider)
		{
			base.Initialise();

            Helper.AssertIsNotNull(buildable, uiManager, activenessDecider);

			Buildable = buildable;
			_uiManager = uiManager;

            _activenessDecider = activenessDecider;
            _activenessDecider.PotentialMatchChange += _activenessDecider_PotentialActivenessChange;

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
            if (PotentialMatchChange != null)
            {
                PotentialMatchChange.Invoke(this, EventArgs.Empty);
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
