using BattleCruisers.Buildables.Buildings.Factories;
using BattleCruisers.Drones;
using BattleCruisers.UI;
using BattleCruisers.UI.BuildMenus;
using BattleCruisers.Buildables.Units;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.UI;

namespace BattleCruisers.Buildables.Buildings.Buttons
{
	public class UnitButtonController : BuildableButtonController
	{
		private Unit _unit;
		private UIManager _uiManager;
		private Factory _factory;

		public Image unitImage;
		public Text unitName;
		public Text droneLevel;

		public void Initialize(Unit unit, IDroneManager droneManager, UIManager uiManager)
		{
			base.Initialize(unit, droneManager);

			_unit = unit;
			_uiManager = uiManager;

			unitName.text = unit.buildableName;
			droneLevel.text = unit.numOfDronesRequired.ToString();
			unitImage.sprite = unit.Sprite;
		}

		public override void OnPresenting(object activationParameter)
		{
			_factory = activationParameter as Factory;
			Assert.IsNotNull(_factory);

			if (_factory.BuildableState != BuildableState.Completed)
			{
				_factory.CompletedBuildable += _factory_CompletedBuildable;
			}

			// Usually have this at the start of the overriding method, but 
			// do not want parent to call ShouldBeEnabled() until we have
			// set our _factory field.
			base.OnPresenting(activationParameter);
		}

		private void _factory_CompletedBuildable(object sender, System.EventArgs e)
		{
			UpdateButtonActiveness();
		}

		public override void OnDismissing()
		{
			base.OnDismissing();

			_factory.CompletedBuildable -= _factory_CompletedBuildable;
			_factory = null;
		}

		protected override bool ShouldBeEnabled()
		{
			return _factory.BuildableState == BuildableState.Completed
				&& base.ShouldBeEnabled();
		}

		protected override void OnClick()
		{
			_factory.Unit = _unit;
			_uiManager.ShowUnitDetails(_unit);
		}
	}
}