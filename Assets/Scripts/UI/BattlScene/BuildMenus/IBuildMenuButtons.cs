using System.Collections.ObjectModel;
using BattleCruisers.Buildables.Buildings;
using BattleCruisers.UI.BattleScene.Buttons;

namespace BattleCruisers.UI.BattleScene.BuildMenus
{
    public interface IBuildMenuButtons
    {
        IBuildingCategoryButton GetCategoryButton(BuildingCategory category);
        ReadOnlyCollection<IBuildableButton> GetBuildableButtons(BuildingCategory category);
    }
}
