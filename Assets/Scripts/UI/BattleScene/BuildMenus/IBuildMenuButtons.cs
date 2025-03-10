using BattleCruisers.Buildables.Buildings;
using BattleCruisers.UI.BattleScene.Buttons;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace BattleCruisers.UI.BattleScene.BuildMenus
{
    public interface IBuildMenuButtons
    {
        IReadOnlyCollection<IBuildableButton> BuildableButtons { get; }

        IBuildingCategoryButton GetBuildingCategoryButton(BuildingCategory category);
        ReadOnlyCollection<IBuildableButton> GetBuildingButtons(BuildingCategory category);
    }
}
