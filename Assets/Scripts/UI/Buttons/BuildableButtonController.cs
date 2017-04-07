using BattleCruisers.Drones;
using BattleCruisers.UI;
using BattleCruisers.Utils;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace BattleCruisers.Buildings.Buttons
{
	public abstract class BuildableButtonController : Presentable
	{
		private Buildable _buildable;
		private IDroneManager _droneManager;
		private Button _button;

		public CanvasGroup canvasGroup;

		public void Initialize(Buildable buildable, IDroneManager droneManager)
		{
			base.Initialize();

			_buildable = buildable;
			_droneManager = droneManager;
			_button = GetComponent<Button>();

			_button.onClick.AddListener(OnClick);
			_droneManager.DroneNumChanged += DroneManager_DroneNumChanged;
		}

		private void DroneManager_DroneNumChanged(object sender, DroneNumChangedEventArgs e)
		{
			UpdateButtonActiveness();
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
