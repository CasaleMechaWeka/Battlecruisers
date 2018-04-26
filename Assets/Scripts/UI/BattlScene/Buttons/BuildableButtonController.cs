using System;
using BattleCruisers.Buildables;
using BattleCruisers.UI.BattleScene.Manager;
using BattleCruisers.Utils;
using UnityEngine;
using UnityEngine.UI;

namespace BattleCruisers.UI.BattleScene.Buttons
{
    public abstract class BuildableButtonController : Presentable
	{
		private IBuildable _buildable;
		protected IUIManager _uiManager;
        private IBuildableButtonActivenessDecider<IBuildable> _activenessDecider;
		private Button _button;

		public CanvasGroup canvasGroup;
		public Image buildableImage;
		public Text buildableName;
		public Text droneLevel;

        public void Initialise(IBuildable buildable, IUIManager uiManager, IBuildableButtonActivenessDecider<IBuildable> activenessDecider)
		{
			base.Initialise();

            Helper.AssertIsNotNull(buildable, uiManager, activenessDecider);

			_buildable = buildable;
			_uiManager = uiManager;
            _activenessDecider = activenessDecider;
			_button = GetComponent<Button>();

            buildableName.text = _buildable.Name;
			droneLevel.text = _buildable.NumOfDronesRequired.ToString();
			buildableImage.sprite = _buildable.Sprite;

			_button.onClick.AddListener(OnClick);
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
			if (ShouldBeEnabled())
			{
				_button.enabled = true;
				canvasGroup.alpha = Constants.ENABLED_UI_ALPHA;
			}
			else
			{
				_button.enabled = false;
				canvasGroup.alpha = Constants.DISABLED_UI_ALPHA;
			}
		}

		protected virtual bool ShouldBeEnabled()
		{
            return _activenessDecider.ShouldBeEnabled(_buildable);
		}

		protected abstract void OnClick();
	}
}
