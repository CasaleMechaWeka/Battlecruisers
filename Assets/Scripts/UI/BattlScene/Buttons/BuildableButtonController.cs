using BattleCruisers.Buildables;
using BattleCruisers.Drones;
using BattleCruisers.Utils;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace BattleCruisers.UI.BattleScene.Buttons
{
	public abstract class BuildableButtonController : Presentable
	{
		private Buildable _buildable;
		private IDroneManager _droneManager;
		protected UIManager _uiManager;
		private Button _button;

		public CanvasGroup canvasGroup;
		public Image buildableImage;
		public Text buildableName;
		public Text droneLevel;

		public void Initialize(Buildable buildable, IDroneManager droneManager, UIManager uiManager)
		{
			base.Initialize();

			_buildable = buildable;
			_droneManager = droneManager;
			_uiManager = uiManager;
			_button = GetComponent<Button>();

			buildableName.text = _buildable.buildableName;
			droneLevel.text = _buildable.numOfDronesRequired.ToString();
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
			return _droneManager.CanSupportDroneConsumer(_buildable.numOfDronesRequired);
		}

		protected abstract void OnClick();
	}
}
