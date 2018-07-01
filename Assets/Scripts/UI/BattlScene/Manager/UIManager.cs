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
    // FELIX  Update tests :)
    public class UIManager : IUIManager
	{
		private readonly ICruiser _playerCruiser, _aiCruiser;
        private readonly IBuildMenu _buildMenu;
        private readonly IBuildableDetailsManager _detailsManager;
        private readonly IFilter<IBuilding> _shouldBuildingDeleteButtonBeEnabledFilter;

        public UIManager(IManagerArgs args)
		{
            Assert.IsNotNull(args);

			_playerCruiser = args.PlayerCruiser;
            _aiCruiser = args.AICruiser;
            _buildMenu = args.BuildMenu;
            _detailsManager = args.DetailsManager;
            _shouldBuildingDeleteButtonBeEnabledFilter = args.ShouldBuildingDeleteButtonBeEnabledFilter;
        }

        /// <summary>
        /// Not in constructor because of circular dependency between:
        /// * UIManager
        /// and 
        /// * Cruisers
        /// * Build menu  
        /// Hence need to wait until all classes are set up before executing this method.
        /// </summary>
        public void InitialUI()
        {
			_detailsManager.HideDetails();
            _buildMenu.ShowBuildMenu();
        }

		public virtual void HideItemDetails()
		{
            _detailsManager.HideDetails();
            _playerCruiser.SlotWrapper.UnhighlightSlots();
            _aiCruiser.SlotWrapper.UnhighlightSlots();
		}

		public void ShowBuildingGroups()
        {
            Logging.Log(Tags.UI_MANAGER, ".ShowBuildingGroups()");

            _playerCruiser.SlotWrapper.UnhighlightSlots();
            _playerCruiser.SlotWrapper.HideAllSlots();
            _detailsManager.HideDetails();
            _buildMenu.ShowBuildingGroupsMenu();
        }

        public void SelectBuildingGroup(BuildingCategory buildingCategory)
		{
			Logging.Log(Tags.UI_MANAGER, ".SelectBuildingGroup()");

			_playerCruiser.SlotWrapper.ShowAllSlots();
			_buildMenu.ShowBuildingGroupMenu(buildingCategory);
		}

		public void SelectBuildingFromMenu(IBuildableWrapper<IBuilding> buildingWrapper)
		{
			Logging.Log(Tags.UI_MANAGER, ".SelectBuildingFromMenu()");
			
            _playerCruiser.SelectedBuildingPrefab = buildingWrapper;
			_playerCruiser.SlotWrapper.HighlightAvailableSlots(buildingWrapper.Buildable.SlotType);
            _detailsManager.ShowDetails(buildingWrapper.Buildable, allowDelete: false);
		}

		public virtual void SelectBuilding(IBuilding building)
		{
            HideItemDetails();

			if (ReferenceEquals(building.ParentCruiser, _playerCruiser))
			{
				SelectBuildingFromFriendlyCruiser(building);
			}
			else if (ReferenceEquals(building.ParentCruiser, _aiCruiser))
            {
				SelectBuildingFromEnemyCruiser(building);
			}
		}

		private void SelectBuildingFromFriendlyCruiser(IBuilding building)
		{
            _playerCruiser.SlotWrapper.HighlightBuildingSlot(building);
            bool allowDelete = _shouldBuildingDeleteButtonBeEnabledFilter.IsMatch(building);
            _detailsManager.ShowDetails(building, allowDelete);
		}

		private void SelectBuildingFromEnemyCruiser(IBuilding building)
		{
            _aiCruiser.SlotWrapper.HighlightBuildingSlot(building);
            bool allowDelete = _shouldBuildingDeleteButtonBeEnabledFilter.IsMatch(building);
            _detailsManager.ShowDetails(building, allowDelete);
		}

		public void ShowFactoryUnits(IFactory factory)
		{
            if (ReferenceEquals(factory.ParentCruiser, _playerCruiser))
			{
				_buildMenu.ShowUnitsMenu(factory);
			}
		}

		public virtual void ShowUnitDetails(IUnit unit)
		{
            HideItemDetails();
            _detailsManager.ShowDetails(unit);
		}

        public virtual void ShowCruiserDetails(ICruiser cruiser)
        {
            HideItemDetails();
            _detailsManager.ShowDetails(cruiser);
        }
    }
}
