using System;
using BattleCruisers.Buildables;
using BattleCruisers.UI.BattleScene.Manager;
using BattleCruisers.Utils;
using UnityEngine.UI;

namespace BattleCruisers.UI.BattleScene.Buttons
{
    public abstract class BuildableButtonController : Presentable, IBuildableButton
	{
		protected IUIManager _uiManager;
        private IActivenessDecider<IBuildable> _activenessDecider;
        private ButtonWrapper _buttonWrapper;

		public Image buildableImage;
		public Text buildableName;
		public Text droneLevel;

        public event EventHandler Clicked;

        public IBuildable Buildable { get; private set; }

        public void Initialise(IBuildable buildable, IUIManager uiManager, IActivenessDecider<IBuildable> activenessDecider)
		{
			base.Initialise();

            Helper.AssertIsNotNull(buildable, uiManager, activenessDecider);

			Buildable = buildable;
			_uiManager = uiManager;
            _activenessDecider = activenessDecider;

            buildableName.text = Buildable.Name;
            droneLevel.text = Buildable.NumOfDronesRequired.ToString();
            buildableImage.sprite = Buildable.Sprite;

			_buttonWrapper = GetComponent<ButtonWrapper>();
            _buttonWrapper.Initialise(HandleClick);
            _activenessDecider.PotentialActivenessChange += _activenessDecider_PotentialActivenessChange;
		}

        private void _activenessDecider_PotentialActivenessChange(object sender, EventArgs e)
        {
			if (_isPresented)
			{
				UpdateButtonActiveness();
			}
        }

		public override void OnPresenting(object activationParameter)
		{
			base.OnPresenting(activationParameter);
			UpdateButtonActiveness();
		}

		protected void UpdateButtonActiveness()
		{
            _buttonWrapper.IsEnabled = ShouldBeEnabled();
		}

		protected virtual bool ShouldBeEnabled()
		{
            return _activenessDecider.ShouldBeEnabled(Buildable);
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
