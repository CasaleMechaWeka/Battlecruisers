using System.Collections.Generic;

namespace BattleCruisers.UI.BattleScene.BuildMenus
{
    public interface IBuildableMenus<TCategories>
    {
        IReadOnlyCollection<IBuildablesMenu> Menus { get; }

        IBuildablesMenu GetBuildablesMenu(TCategories buildableCategory);
    }
}