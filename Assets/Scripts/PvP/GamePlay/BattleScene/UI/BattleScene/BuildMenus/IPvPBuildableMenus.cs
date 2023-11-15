using System.Collections.Generic;

namespace BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.UI.BattleScene.BuildMenus
{
    public interface IPvPBuildableMenus<TCategories>
    {
        IReadOnlyCollection<IPvPBuildablesMenu> Menus { get; }

        IPvPBuildablesMenu GetBuildablesMenu(TCategories buildableCategory);
    }
}