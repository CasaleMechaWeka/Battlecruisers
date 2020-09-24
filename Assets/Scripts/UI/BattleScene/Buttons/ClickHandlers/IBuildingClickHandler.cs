using BattleCruisers.Buildables;
using BattleCruisers.Buildables.Buildings;

namespace BattleCruisers.UI.BattleScene.Buttons.ClickHandlers
{
    public interface IBuildingClickHandler
    {
        void HandleClick(bool canAffordBuildable, IBuildableWrapper<IBuilding> buildingClicked);
    }
}