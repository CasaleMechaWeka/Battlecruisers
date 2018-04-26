using System;
using BattleCruisers.Buildables;
using BattleCruisers.UI.BattleScene.Manager;
using BattleCruisers.Utils;
using UnityEngine.UI;

namespace BattleCruisers.UI.BattleScene.Buttons
{
    public abstract class BuildableButtonController : Presentable
	{
		private IBuildable _buildable;
		protected IUIManager _uiManager;
        private IActivenessDecider<IBuildable> _activenessDecider;
        private ButtonWrapper _buttonWrapper;

		public Image buildableImage;
		public Text buildableName;
		public Text droneLevel;

        public void Initialise(IBuildable buildable, IUIManager uiManager, IActivenessDecider<IBuildable> activenessDecider)
		{
			base.Initialise();

            Helper.AssertIsNotNull(buildable, uiManager, activenessDecider);

			_buildable = buildable;
			_uiManager = uiManager;
            _activenessDecider = activenessDecider;

            buildableName.text = _buildable.Name;
            droneLevel.text = _buildable.NumOfDronesRequired.ToString();
            buildableImage.sprite = _buildable.Sprite;

			_buttonWrapper = GetComponent<ButtonWrapper>();
            _buttonWrapper.Initialise();
			_buttonWrapper.Button.onClick.AddListener(OnClick);
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
            return _activenessDecider.ShouldBeEnabled(_buildable);
		}

		protected abstract void OnClick();
	}
}
