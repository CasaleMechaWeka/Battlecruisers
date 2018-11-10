using BattleCruisers.Buildables;
using BattleCruisers.Buildables.Buildings;
using BattleCruisers.Buildables.Buildings.Factories;
using BattleCruisers.Buildables.Units;
using BattleCruisers.Cruisers;
using BattleCruisers.UI.BattleScene.BuildMenus;
using BattleCruisers.UI.Common.BuildableDetails;
using BattleCruisers.Utils;
using UnityEngine;
using UnityEngine.Assertions;

namespace BattleCruisers.UI.BattleScene.Manager
{
    // NEWUI  Imlement :D
    // NEWUI  Update tests :)
    public class UIManagerNEW : IUIManager
	{
		private readonly ICruiser _playerCruiser, _aiCruiser;
        private readonly IBuildMenuNEW _buildMenu;
        private readonly IItemDetailsManager _detailsManager;

        // FELIX  Update IManagerArgs
        // FELIX  Use IManagerArgs :)
        public UIManagerNEW(
            IBuildMenuNEW buildMenu,
            IItemDetailsManager detailsManager,
            ICruiser playerCruiser,
            ICruiser aiCruiser)
        {
            Helper.AssertIsNotNull(buildMenu, detailsManager, playerCruiser, aiCruiser);

            _buildMenu = buildMenu;
            _detailsManager = detailsManager;
            _playerCruiser = playerCruiser;
            _aiCruiser = aiCruiser;
        }

  //      public UIManagerNEW(IManagerArgs args)
		//{
  //          Assert.IsNotNull(args);

		//	_playerCruiser = args.PlayerCruiser;
  //          _aiCruiser = args.AICruiser;
  //          _buildMenu = args.BuildMenu;
  //          _detailsManager = args.DetailsManager;
  //      }

        /// <summary>
        /// Not in constructor because of circular dependency between:
        /// * UIManager
        /// and 
        /// * Cruisers
        /// * Build menu  
        /// Hence need to wait until all classes are set up before executing this method.
        /// </summary>
        /// NEWUI  Remove?
        public void InitialUI()
        {
			//_detailsManager.HideDetails();
   //         _buildMenu.ShowBuildMenu();
        }

		public virtual void HideItemDetails()
		{
            Logging.Log(Tags.UI_MANAGER, ".HideItemDetails()");

            _detailsManager.HideDetails();
            _playerCruiser.SlotHighlighter.UnhighlightSlots();
            _aiCruiser.SlotHighlighter.UnhighlightSlots();
        }

        // FELIX  Rename, HideCurrentlyShownMenu?
		public void ShowBuildingGroups()
        {
            Logging.Log(Tags.UI_MANAGER, ".ShowBuildingGroups()");

            _playerCruiser.SlotHighlighter.UnhighlightSlots();
            _detailsManager.HideDetails();
            _buildMenu.HideCurrentlyShownMenu();
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
            _playerCruiser.SlotHighlighter.HighlightBuildingSlot(building);
            _detailsManager.ShowDetails(building);
		}

		private void SelectBuildingFromEnemyCruiser(IBuilding building)
		{
            _aiCruiser.SlotHighlighter.HighlightBuildingSlot(building);
            _detailsManager.ShowDetails(building);
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
        }

        public virtual void ShowCruiserDetails(ICruiser cruiser)
        {
			Logging.Log(Tags.UI_MANAGER, ".ShowCruiserDetails()");

            HideItemDetails();
            _detailsManager.ShowDetails(cruiser);
        }
    }
}
