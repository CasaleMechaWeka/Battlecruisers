using BattleCruisers.Buildables.Buildings;
using BattleCruisers.Buildables.Units;
using BattleCruisers.Drones;
using BattleCruisers.Fetchers;
using BattleCruisers.UI.BattleScene.BuildMenus;
using BattleCruisers.UI.BattleScene.Buttons;
using BattleCruisers.Utils;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace BattleCruisers.UI.BattleScene
{
	public interface IUIFactory
	{
		GameObject CreatePanel(bool isActive);
		Button CreateBuildingCategoryButton(HorizontalLayoutGroup buttonParent, BuildingGroup group);
		BuildingButtonController CreateBuildingButton(HorizontalLayoutGroup buttonParent, BuildingWrapper buildingWrapper);
		UnitButtonController CreateUnitButton(HorizontalLayoutGroup buttonParent, UnitWrapper unitWrapper);
		Button CreateBackButton(HorizontalLayoutGroup buttonParent);
	}

	public class UIFactory : MonoBehaviour, IUIFactory
	{
		private Canvas _canvas;
		private ISpriteFetcher _spriteFetcher;
		private IDroneManager _droneManager;

		public UIManager uiManager;
		public GameObject panelPrefab;
		public Button buildingCategoryButtonPrefab;
		public Button buildingButtonPrefab;
		public Button unitButtonPrefab;
		public Button backButtonPrefab;

		public void Awake()
		{
			_canvas = GetComponent<Canvas>();
		}

		public void Initialise(ISpriteFetcher spriteFetcher, IDroneManager droneManager)
		{
			_spriteFetcher = spriteFetcher;
			_droneManager = droneManager;
		}

		public GameObject CreatePanel(bool isActive)
		{
			GameObject panel = Instantiate(panelPrefab);
			panel.SetActive(isActive);
			panel.transform.SetParent(_canvas.transform);
			RectTransform rectTransform = panel.GetComponent<RectTransform>();
			rectTransform.anchoredPosition = new Vector2(-25, 25);
			return panel;
		}

		public Button CreateBuildingCategoryButton(HorizontalLayoutGroup buttonParent, BuildingGroup group)
		{
			Button button = (Button)Instantiate(buildingCategoryButtonPrefab);
			button.transform.SetParent(buttonParent.transform, worldPositionStays: false);
			button.GetComponent<BuildingCategoryButton>().Initialize(group, uiManager);
			return button;
		}

		public BuildingButtonController CreateBuildingButton(HorizontalLayoutGroup buttonParent, BuildingWrapper buildingWrapper)
		{
			Button button = (Button)Instantiate(buildingButtonPrefab);
			button.transform.SetParent(buttonParent.transform, worldPositionStays: false);
			Sprite slotSprite = _spriteFetcher.GetSlotSprite(buildingWrapper.building.slotType);
			BuildingButtonController controller = button.GetComponent<BuildingButtonController>();
			controller.Initialize(buildingWrapper, uiManager, _droneManager, slotSprite);
			return controller;
		}

		public UnitButtonController CreateUnitButton(HorizontalLayoutGroup buttonParent, UnitWrapper unitWrapper)
		{
			Button button = (Button)Instantiate(unitButtonPrefab);
			button.transform.SetParent(buttonParent.transform, worldPositionStays: false);
			UnitButtonController controller = button.GetComponent<UnitButtonController>();
			controller.Initialize(unitWrapper, _droneManager, uiManager);
			return controller;
		}

		public Button CreateBackButton(HorizontalLayoutGroup buttonParent)
		{
			Button backButton = (Button)Instantiate(backButtonPrefab);
			backButton.transform.SetParent(buttonParent.transform, worldPositionStays: false);
			backButton.GetComponent<BackButtonController>().Initialize(uiManager);
			return backButton;
		}
	}
}
