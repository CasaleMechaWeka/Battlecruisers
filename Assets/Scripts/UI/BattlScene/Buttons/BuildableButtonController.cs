using BattleCruisers.Buildables;
using BattleCruisers.Cruisers.Drones;
using BattleCruisers.UI.BattleScene.Manager;
using BattleCruisers.Utils;
using UnityEngine;
using UnityEngine.UI;

namespace BattleCruisers.UI.BattleScene.Buttons
{
    public abstract class BuildableButtonController : Presentable
	{
		private IBuildable _buildable;
		private IDroneManager _droneManager;
		protected IUIManager _uiManager;
		private Button _button;

		public CanvasGroup canvasGroup;
		public Image buildableImage;
		public Text buildableName;
		public Text droneLevel;

		public void Initialise(IBuildable buildable, IDroneManager droneManager, IUIManager uiManager)
		{
			base.Initialise();

            Helper.AssertIsNotNull(buildable, droneManager, uiManager);

			_buildable = buildable;
			_droneManager = droneManager;
			_uiManager = uiManager;
			_button = GetComponent<Button>();

            buildableName.text = _buildable.Name;
			droneLevel.text = _buildable.NumOfDronesRequired.ToString();
			buildableImage.sprite = _buildable.Sprite;

			_button.onClick.AddListener(OnClick);
			_droneManager.DroneNumChanged += DroneManager_DroneNumChanged;
		}

		private void DroneManager_DroneNumChanged(object sender, DroneNumChangedEventArgs e)
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
			return _droneManager.CanSupportDroneConsumer(_buildable.NumOfDronesRequired);
		}

		protected abstract void OnClick();
	}
}
