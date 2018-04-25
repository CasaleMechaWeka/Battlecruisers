using BattleCruisers.Buildables;
using BattleCruisers.Buildables.Buildings;
using BattleCruisers.Buildables.Units;
using BattleCruisers.Cruisers.Drones;
using BattleCruisers.UI.BattleScene.Buttons;
using BattleCruisers.UI.BattleScene.Manager;
using BattleCruisers.Utils;
using BattleCruisers.Utils.Fetchers;
using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.UI;

namespace BattleCruisers.UI.BattleScene
{
	public class UIFactory : MonoBehaviour, IUIFactory
	{
        private IUIManager _uiManager;
        private ISpriteProvider _spriteProvider;
        private IDroneManager _droneManager;
		private Canvas _canvas;

		public GameObject panelPrefab;
		public Button buildingCategoryButtonPrefab;
		public Button buildingButtonPrefab;
		public Button unitButtonPrefab;
		public Button backButtonPrefab;

        public void Initialise(IUIManager uiManager, ISpriteProvider spriteProvider, IDroneManager droneManager)
        {
            Helper.AssertIsNotNull(uiManager, spriteProvider, droneManager);

            _uiManager = uiManager;
            _spriteProvider = spriteProvider;
            _droneManager = droneManager;
			
            _canvas = GetComponent<Canvas>();
            Assert.IsNotNull(_canvas);
		}

		public GameObject CreatePanel(bool isActive)
		{
			GameObject panel = Instantiate(panelPrefab);
			panel.SetActive(isActive);
            panel.transform.SetParent(_canvas.transform, worldPositionStays: false);
			RectTransform rectTransform = panel.GetComponent<RectTransform>();
			rectTransform.anchoredPosition = new Vector2(-25, 25);
			return panel;
		}

        public Button CreateBuildingCategoryButton(HorizontalLayoutGroup buttonParent, IBuildingGroup buildingGroup)
		{
			Button button = Instantiate(buildingCategoryButtonPrefab);
			button.transform.SetParent(buttonParent.transform, worldPositionStays: false);
			button.GetComponent<BuildingCategoryButton>().Initialize(buildingGroup, _uiManager);
			return button;
		}

		public BuildingButtonController CreateBuildingButton(HorizontalLayoutGroup buttonParent, IBuildableWrapper<IBuilding> buildingWrapper)
		{
			Button button = Instantiate(buildingButtonPrefab);
			button.transform.SetParent(buttonParent.transform, worldPositionStays: false);
            Sprite slotSprite = _spriteProvider.GetSlotSprite(buildingWrapper.Buildable.SlotType).Sprite;
			BuildingButtonController controller = button.GetComponent<BuildingButtonController>();
			controller.Initialize(buildingWrapper, _uiManager, _droneManager, slotSprite);
			return controller;
		}

		public UnitButtonController CreateUnitButton(HorizontalLayoutGroup buttonParent, IBuildableWrapper<IUnit> unitWrapper)
		{
			Button button = Instantiate(unitButtonPrefab);
			button.transform.SetParent(buttonParent.transform, worldPositionStays: false);
			UnitButtonController controller = button.GetComponent<UnitButtonController>();
			controller.Initialize(unitWrapper, _droneManager, _uiManager);
			return controller;
		}

		public Button CreateBackButton(HorizontalLayoutGroup buttonParent)
		{
			Button backButton = Instantiate(backButtonPrefab);
			backButton.transform.SetParent(buttonParent.transform, worldPositionStays: false);
			backButton.GetComponent<BackButtonController>().Initialize(_uiManager);
			return backButton;
		}
	}
}
