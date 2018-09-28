using BattleCruisers.Buildables;
using BattleCruisers.Buildables.Buildings.Factories;
using BattleCruisers.Buildables.Units;

namespace BattleCruisers.UI.BattleScene.Buttons.ClickHandlers
{
    public interface IUnitClickHandler
    {
        // FELIX  Rename.  Remove "unit" :P
        void HandleUnitClick(IBuildableWrapper<IUnit> unitClicked, IFactory unitFactory);
    }
}