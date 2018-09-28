using BattleCruisers.Buildables;
using BattleCruisers.Buildables.Buildings.Factories;
using BattleCruisers.Buildables.Units;

namespace BattleCruisers.UI.BattleScene.Buttons.ClickHandlers
{
    public interface IUnitClickHandler
    {
        void HandleClick(bool canAffordBuildable, IBuildableWrapper<IUnit> unitClicked, IFactory unitFactory);
    }
}