using BattleCruisers.Buildables.Buildings;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.UI.BattleScene.Buttons;
using BattleCruisers.UI.BattleScene.Buttons;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.UI.BattleScene.BuildMenus
{
    public interface IPvPBuildMenuButtons
    {
        IReadOnlyCollection<IPvPBuildableButton> BuildableButtons { get; }

        IBuildingCategoryButton GetBuildingCategoryButton(BuildingCategory category);
        ReadOnlyCollection<IPvPBuildableButton> GetBuildingButtons(BuildingCategory category);
    }
}
