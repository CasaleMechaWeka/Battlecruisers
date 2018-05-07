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
        private IFilter<IBuildable> _buildableButtonActivenessDecider;
        private IFilter<BuildingCategory> _buildingCategoryButtonActivenessDecider;
        private IFilter _backButtonDecider;
		private Canvas _canvas;

		public GameObject panelPrefab;
        public BuildingCategoryButton buildingCategoryButtonPrefab;
		public Button buildingButtonPrefab;
		public Button unitButtonPrefab;
		public Button backButtonPrefab;

        public void Initialise(
            IUIManager uiManager, 
            ISpriteProvider spriteProvider, 
            IFilter<IBuildable> buildableButtonActivenessDecider,
            IFilter<BuildingCategory> buildingCategoryButtonActivenessDecider,
            IFilter backButtonDecider)
        {
            Helper.AssertIsNotNull(
                uiManager, 
                spriteProvider, 
                buildableButtonActivenessDecider, 
                buildingCategoryButtonActivenessDecider, 
                backButtonDecider);

            _uiManager = uiManager;
            _spriteProvider = spriteProvider;
            _buildableButtonActivenessDecider = buildableButtonActivenessDecider;
            _buildingCategoryButtonActivenessDecider = buildingCategoryButtonActivenessDecider;
            _backButtonDecider = backButtonDecider;

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

        public IBuildingCategoryButton CreateBuildingCategoryButton(HorizontalLayoutGroup buttonParent, IBuildingGroup buildingGroup)
		{
            BuildingCategoryButton button = Instantiate(buildingCategoryButtonPrefab);
			button.transform.SetParent(buttonParent.transform, worldPositionStays: false);
			button.Initialise(buildingGroup, _uiManager, _buildingCategoryButtonActivenessDecider);
			return button;
		}

		public BuildingButtonController CreateBuildingButton(HorizontalLayoutGroup buttonParent, IBuildableWrapper<IBuilding> buildingWrapper)
		{
			Button button = Instantiate(buildingButtonPrefab);
			button.transform.SetParent(buttonParent.transform, worldPositionStays: false);
            Sprite slotSprite = _spriteProvider.GetSlotSprite(buildingWrapper.Buildable.SlotType).Sprite;
			BuildingButtonController controller = button.GetComponent<BuildingButtonController>();
            controller.Initialise(buildingWrapper, _uiManager, _buildableButtonActivenessDecider, slotSprite);
			return controller;
		}

		public UnitButtonController CreateUnitButton(HorizontalLayoutGroup buttonParent, IBuildableWrapper<IUnit> unitWrapper)
		{
			Button button = Instantiate(unitButtonPrefab);
			button.transform.SetParent(buttonParent.transform, worldPositionStays: false);
			UnitButtonController controller = button.GetComponent<UnitButtonController>();
            controller.Initialise(unitWrapper, _uiManager, _buildableButtonActivenessDecider);
			return controller;
		}

		public Button CreateBackButton(HorizontalLayoutGroup buttonParent)
		{
			Button backButton = Instantiate(backButtonPrefab);
			backButton.transform.SetParent(buttonParent.transform, worldPositionStays: false);
			backButton.GetComponent<BackButtonController>().Initialise(_uiManager, _backButtonDecider);
			return backButton;
		}
	}
}
