using BattleCruisers.Buildables;
using BattleCruisers.Buildables.Buildings.Factories;
using BattleCruisers.Buildables.Units;
using BattleCruisers.UI.BattleScene.Manager;
using BattleCruisers.Utils;
using UnityEngine.Assertions;

namespace BattleCruisers.UI.BattleScene.Buttons.ClickHandlers
{
    // FELIX  Add not enough drones sound
    // FELIX  Avoid duplicate code with BuildingClickHandler :)
    public class UnitClickHandler : IUnitClickHandler
    {
        private readonly IUIManager _uiManager;

        public UnitClickHandler(IUIManager uiManager)
        {
            Assert.IsNotNull(uiManager);
            _uiManager = uiManager;
        }

        public void HandleClick(IBuildableWrapper<IUnit> unitClicked, IFactory unitFactory)
        {
            Helper.AssertIsNotNull(unitClicked, unitFactory);

            HandleFactory(unitClicked, unitFactory);
			_uiManager.ShowUnitDetails(unitClicked.Buildable);
        }

        private void HandleFactory(IBuildableWrapper<IUnit> unitClicked, IFactory unitFactory)
        {
            if (ReferenceEquals(unitFactory.UnitWrapper, unitClicked))
            {
                // Same unit
                if (unitFactory.IsUnitPaused.Value)
                {
                    unitFactory.ResumeBuildingUnit();
                }
                else
                {
                    unitFactory.PauseBuildingUnit();
                }
            }
            else
            {
                // Different unit
                unitFactory.StartBuildingUnit(unitClicked);
            }
        }
    }
}