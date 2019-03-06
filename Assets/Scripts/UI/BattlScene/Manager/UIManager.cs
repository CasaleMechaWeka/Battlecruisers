using BattleCruisers.Buildables;
using BattleCruisers.Buildables.Buildings;
using BattleCruisers.Buildables.Buildings.Factories;
using BattleCruisers.Buildables.Units;
using BattleCruisers.Cruisers;
using BattleCruisers.UI.BattleScene.BuildMenus;
using BattleCruisers.UI.Common.BuildableDetails;
using BattleCruisers.Utils;
using UnityEngine.Assertions;

namespace BattleCruisers.UI.BattleScene.Manager
{
    public class UIManager : IUIManager
	{
		private ICruiser _playerCruiser, _aiCruiser;
        private IBuildMenu _buildMenu;
        private IItemDetailsManager _detailsManager;

        private ITarget _shownItem;
        private ITarget ShownItem
        {
            set
            {
                if (_shownItem != null)
                {
                    _shownItem.Destroyed -= _shownItem_Destroyed;
                }

                _shownItem = value;

                if (_shownItem != null)
                {
                    _shownItem.Destroyed += _shownItem_Destroyed;
                }
            }
        }

        // Not in constructor because of circular dependency with:
        // + Build menu
        // + Cruisers
        public void Initialise(ManagerArgs args)
        {
            Assert.IsNotNull(args);

            _buildMenu = args.BuildMenu;
            _detailsManager = args.DetailsManager;
            _playerCruiser = args.PlayerCruiser;
            _aiCruiser = args.AICruiser;
        }

        private void _shownItem_Destroyed(object sender, DestroyedEventArgs e)
        {
            HideItemDetails();
        }

		public virtual void HideItemDetails()
		{
            Logging.Log(Tags.UI_MANAGER, ".HideItemDetails()");

            _detailsManager.HideDetails();
            _playerCruiser.SlotHighlighter.UnhighlightSlots();
            _aiCruiser.SlotHighlighter.UnhighlightSlots();
            ShownItem = null;
        }

		public void HideCurrentlyShownMenu()
        {
            Logging.Log(Tags.UI_MANAGER, ".HideCurrentlyShownMenu()");

            _detailsManager.HideDetails();
            _playerCruiser.SlotHighlighter.UnhighlightSlots();
            _buildMenu.HideCurrentlyShownMenu();
            ShownItem = null;
        }

        public void SelectBuildingGroup(BuildingCategory buildingCategory)
		{
			Logging.Log(Tags.UI_MANAGER, ".SelectBuildingGroup()");

            _buildMenu.ShowBuildingGroupMenu(buildingCategory);
        }

		public void SelectBuildingFromMenu(IBuildableWrapper<IBuilding> buildingWrapper)
		{
			Logging.Log(Tags.UI_MANAGER, ".SelectBuildingFromMenu()");

            _playerCruiser.SelectedBuildingPrefab = buildingWrapper;
            _playerCruiser.SlotHighlighter.HighlightAvailableSlots(buildingWrapper.Buildable.SlotSpecification.SlotType);
            _detailsManager.ShowDetails(buildingWrapper.Buildable);
        }

		public virtual void SelectBuilding(IBuilding building)
		{
			Logging.Log(Tags.UI_MANAGER, ".SelectBuilding()");

            HideItemDetails();
            _detailsManager.ShowDetails(building);
            ShownItem = building;
        }

		public void ShowFactoryUnits(IFactory factory)
		{
			Logging.Log(Tags.UI_MANAGER, ".SelectFactoryUnits()");

            if (ReferenceEquals(factory.ParentCruiser, _playerCruiser))
            {
                _buildMenu.ShowUnitsMenu(factory);
            }
        }

		public virtual void ShowUnitDetails(IUnit unit)
		{
            Logging.Log(Tags.UI_MANAGER, ".ShowUnitDetails()");

            HideItemDetails();
            _detailsManager.ShowDetails(unit);
            ShownItem = unit;
        }

        public virtual void ShowCruiserDetails(ICruiser cruiser)
        {
			Logging.Log(Tags.UI_MANAGER, ".ShowCruiserDetails()");

            HideItemDetails();
            _detailsManager.ShowDetails(cruiser);
            ShownItem = cruiser;
        }
    }
}
