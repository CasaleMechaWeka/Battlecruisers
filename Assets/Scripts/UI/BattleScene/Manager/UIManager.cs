using BattleCruisers.Buildables;
using BattleCruisers.Buildables.Buildings;
using BattleCruisers.Buildables.Buildings.Factories;
using BattleCruisers.Buildables.Units;
using BattleCruisers.Cruisers;
using BattleCruisers.Data.Static;
using BattleCruisers.Tutorial.Explanation;
using BattleCruisers.UI.BattleScene.BuildMenus;
using BattleCruisers.UI.BattleScene.InGameHints;
using BattleCruisers.UI.Common.BuildableDetails;
using BattleCruisers.UI.Sound.Players;
using BattleCruisers.Utils;
using BattleCruisers.Utils.Localisation;
using UnityEngine;
using UnityEngine.Assertions;

namespace BattleCruisers.UI.BattleScene.Manager
{
    public class UIManager : IUIManager
	{
		private ICruiser _playerCruiser, _aiCruiser;
        private IBuildMenu _buildMenu;
        private IItemDetailsManager _detailsManager;
        private IPrioritisedSoundPlayer _soundPlayer;
        private ISingleSoundPlayer _uiSoundPlayer;
        private IBuilding lastClickedBuilding;
        private IUnit lastClickedUnit;
        private ICruiser lastClickedCruiser;
        private ITarget lastClickedBuildable;
        private int lastClickedType = -1; //-1 is for nothing, 0 is for buildings and 1 is for units

        private ITarget _shownItem;
        private IExplanationPanel _explanationPanel;
        private IHintDisplayer _hintDisplayer;

        public void SetExplanationPanel(IExplanationPanel explanationPanelValue) {
            _explanationPanel = explanationPanelValue;
        }

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
            _soundPlayer = args.SoundPlayer;
            _uiSoundPlayer = args.UISoundPlayer;
        }

        private void _shownItem_Destroyed(object sender, DestroyedEventArgs e)
        {
            HideItemDetails();
        }

		public virtual void HideItemDetails()
		{
            Logging.LogMethod(Tags.UI_MANAGER);

            _detailsManager.HideDetails();
            _playerCruiser.SlotHighlighter.UnhighlightSlots();
            _aiCruiser.SlotHighlighter.UnhighlightSlots();
            ShownItem = null;
            lastClickedBuilding = null;
            lastClickedUnit = null;
            lastClickedType = -1;
            lastClickedBuildable = null;
        }

		public void HideCurrentlyShownMenu()
        {
            Logging.LogMethod(Tags.UI_MANAGER);

            _detailsManager.HideDetails();
            _playerCruiser.SlotHighlighter.UnhighlightSlots();
            _buildMenu.HideCurrentlyShownMenu();
            ShownItem = null;
            lastClickedBuilding = null;
            lastClickedUnit = null;
            lastClickedType = -1;
            lastClickedBuildable = null;
        }

        public void SelectBuildingGroup(BuildingCategory buildingCategory)
		{
            Logging.LogMethod(Tags.UI_MANAGER);

            HideItemDetails();
            _buildMenu.ShowBuildingGroupMenu(buildingCategory);
        }

		public async void SelectBuildingFromMenu(IBuildableWrapper<IBuilding> buildingWrapper)
		{
            Logging.LogMethod(Tags.UI_MANAGER);

            _playerCruiser.SelectedBuildingPrefab = buildingWrapper;
            //_detailsManager.ShowDetails(buildingWrapper.Buildable);
            bool wasAnySlotHighlighted =_playerCruiser.SlotHighlighter.HighlightAvailableSlots(buildingWrapper.Buildable.SlotSpecification);
            ILocTable _commonStrings = await LocTableFactory.Instance.LoadTutorialTableAsync();
            if (_explanationPanel != null)
            {
                if (_hintDisplayer == null)
                {
                    _hintDisplayer = new NonRepeatingHintDisplayer(new HintDisplayer(_explanationPanel));
                }
                _hintDisplayer.ShowHint(_commonStrings.GetString("Steps/Touchdrag"));
            }
            if (!wasAnySlotHighlighted)
            {
                _soundPlayer.PlaySound(PrioritisedSoundKeys.Events.Cruiser.NoBuildingSlotsLeft);
                _playerCruiser.SlotHighlighter.HighlightSlots(buildingWrapper.Buildable.SlotSpecification);
            }
        }

		public virtual void SelectBuilding(IBuilding building)
		{
            Logging.LogMethod(Tags.UI_MANAGER);

            _detailsManager.ShowDetails(building);
            _detailsManager.SelectBuilding(building);
            ShownItem = building;
            lastClickedBuilding = building;
            lastClickedBuildable = building;
            lastClickedType = 0;
        }

		public void ShowFactoryUnits(IFactory factory)
		{
            Logging.LogMethod(Tags.UI_MANAGER);

            if (ReferenceEquals(factory.ParentCruiser, _playerCruiser))
            {
                _buildMenu.ShowUnitsMenu(factory);
                _uiSoundPlayer.PlaySound(factory.SelectedSound);
            }
        }

		public virtual void ShowUnitDetails(IUnit unit)
		{
            Logging.LogMethod(Tags.UI_MANAGER);

            _detailsManager.ShowDetails(unit);
            _detailsManager.SelectUnit(unit);
            ShownItem = unit;
            lastClickedUnit = unit;
            lastClickedBuildable = unit;
            lastClickedType = 1;
        }

        public virtual void ShowCruiserDetails(ICruiser cruiser)
        {
            Logging.LogMethod(Tags.UI_MANAGER);

            _detailsManager.ShowDetails(cruiser);
            ShownItem = cruiser;
            lastClickedCruiser = cruiser;
            lastClickedBuildable = cruiser;
            lastClickedType = 2;
        }

        public void PeakBuildingDetails(IBuilding building)
        {
            _detailsManager.ShowDetails(building);
            //ShownItem = building;
        }

        public void PeakUnitDetails(IUnit unit)
        {
            _detailsManager.ShowDetails(unit);
            //ShownItem = unit;
        }

        public void UnpeakBuildingDetails()
        {
            if (lastClickedBuildable!=null)
            {
                if (lastClickedType == 0)
                {
                    _detailsManager.ShowDetails(lastClickedBuilding);
                }
                else if (lastClickedType == 1){
                    _detailsManager.ShowDetails(lastClickedUnit);
                }
                else{
                    _detailsManager.ShowDetails(lastClickedCruiser);
                }
                
            }
            else{
                _detailsManager.HideDetails();
            }
        }

        public void UnpeakUnitDetails()
        {
            if (lastClickedBuildable!=null)
            {
                if (lastClickedType == 0)
                {
                    _detailsManager.ShowDetails(lastClickedBuilding);
                }
                else if (lastClickedType == 1){
                    _detailsManager.ShowDetails(lastClickedUnit);
                }
                else{
                    _detailsManager.ShowDetails(lastClickedCruiser);
                }
                
            }
            else{
                _detailsManager.HideDetails();
            }
        }
    }
}
