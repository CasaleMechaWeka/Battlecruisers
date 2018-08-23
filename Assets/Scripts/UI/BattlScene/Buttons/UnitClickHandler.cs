using BattleCruisers.Buildables;
using BattleCruisers.Buildables.Buildings.Factories;
using BattleCruisers.Buildables.Units;
using BattleCruisers.Utils;

namespace BattleCruisers.UI.BattleScene.Buttons
{
    public class UnitClickHandler : IUnitClickHandler
    {
        public void HandleUnitClick(IBuildableWrapper<IUnit> unitClicked, IFactory unitFactory)
        {
            Helper.AssertIsNotNull(unitClicked, unitFactory);

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