using BattleCruisers.Buildables;
using BattleCruisers.Buildables.Buildings;
using BattleCruisers.Buildables.Units;
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
        private IBuildableButtonActivenessDecider _activenessDecider;
		private Canvas _canvas;

		public GameObject panelPrefab;
		public Button buildingCategoryButtonPrefab;
		public Button buildingButtonPrefab;
		public Button unitButtonPrefab;
		public Button backButtonPrefab;

        public void Initialise(IUIManager uiManager, ISpriteProvider spriteProvider, IBuildableButtonActivenessDecider activenessDecider)
        {
            Helper.AssertIsNotNull(uiManager, spriteProvider, activenessDecider);

            _uiManager = uiManager;
            _spriteProvider = spriteProvider;
            _activenessDecider = activenessDecider;
			
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
			button.GetComponent<BuildingCategoryButton>().Initialise(buildingGroup, _uiManager);
			return button;
		}

		public BuildingButtonController CreateBuildingButton(HorizontalLayoutGroup buttonParent, IBuildableWrapper<IBuilding> buildingWrapper)
		{
			Button button = Instantiate(buildingButtonPrefab);
			button.transform.SetParent(buttonParent.transform, worldPositionStays: false);
            Sprite slotSprite = _spriteProvider.GetSlotSprite(buildingWrapper.Buildable.SlotType).Sprite;
			BuildingButtonController controller = button.GetComponent<BuildingButtonController>();
            controller.Initialise(buildingWrapper, _uiManager, _activenessDecider, slotSprite);
			return controller;
		}

		public UnitButtonController CreateUnitButton(HorizontalLayoutGroup buttonParent, IBuildableWrapper<IUnit> unitWrapper)
		{
			Button button = Instantiate(unitButtonPrefab);
			button.transform.SetParent(buttonParent.transform, worldPositionStays: false);
			UnitButtonController controller = button.GetComponent<UnitButtonController>();
            controller.Initialise(unitWrapper, _uiManager, _activenessDecider);
			return controller;
		}

		public Button CreateBackButton(HorizontalLayoutGroup buttonParent)
		{
			Button backButton = Instantiate(backButtonPrefab);
			backButton.transform.SetParent(buttonParent.transform, worldPositionStays: false);
			backButton.GetComponent<BackButtonController>().Initialise(_uiManager);
			return backButton;
		}
	}
}
