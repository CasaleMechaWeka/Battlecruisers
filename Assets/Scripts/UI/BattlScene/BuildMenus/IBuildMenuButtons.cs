using BattleCruisers.Buildables.Buildings;
using BattleCruisers.UI.BattleScene.Buttons;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace BattleCruisers.UI.BattleScene.BuildMenus
{
    public interface IBuildMenuButtons
    {
        IReadOnlyCollection<IBuildableButton> BuildableButtons { get; }

        // FELIX  Rename to make clear these are for buildings :)
        IBuildingCategoryButton GetCategoryButton(BuildingCategory category);
        ReadOnlyCollection<IBuildableButton> GetBuildableButtons(BuildingCategory category);
    }
}
