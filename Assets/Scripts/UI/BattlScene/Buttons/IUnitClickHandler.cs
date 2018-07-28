using BattleCruisers.Buildables;
using BattleCruisers.Buildables.Buildings.Factories;
using BattleCruisers.Buildables.Units;

namespace BattleCruisers.UI.BattleScene.Buttons
{
    public interface IUnitClickHandler
    {
        void HandleUnitClick(IBuildableWrapper<IUnit> unitClicked, IFactory unitFactory);
    }
}