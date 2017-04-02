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
		private IDroneConsumer _droneConsumer;
		private IDroneManager _droneManager;
		private Button _button;

		public CanvasGroup canvasGroup;

		public void Initialize(Buildable buildable, IDroneManager droneManager)
		{
			base.Initialize();

			_droneConsumer = buildable.DroneConsumer;
			_droneManager = droneManager;
			_button = GetComponent<Button>();
		}

		public override void OnPresenting(object activationParameter)
		{
			base.OnPresenting(activationParameter);

			if (_droneManager.CanSupportDroneConsumer(_droneConsumer))
			{
				_button.enabled = true;
				_button.onClick.AddListener(OnClick);
			}
			else
			{
				_button.enabled = false;
				canvasGroup.alpha = Constants.DISABLED_UI_ALPHA;
			}
		}

		public override void OnDismissing()
		{
			base.OnDismissing();

			_button.onClick.RemoveListener(OnClick);
			canvasGroup.alpha = Constants.ENABLED_UI_ALPHA;
		}

		protected abstract void OnClick();
	}
}
