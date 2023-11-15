using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Buildables.Buildings;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.UI.BattleScene.Buttons;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.UI.BattleScene.BuildMenus
{
    public interface IPvPBuildMenuButtons
    {
        IReadOnlyCollection<IPvPBuildableButton> BuildableButtons { get; }

        IPvPBuildingCategoryButton GetBuildingCategoryButton(PvPBuildingCategory category);
        ReadOnlyCollection<IPvPBuildableButton> GetBuildingButtons(PvPBuildingCategory category);
    }
}
