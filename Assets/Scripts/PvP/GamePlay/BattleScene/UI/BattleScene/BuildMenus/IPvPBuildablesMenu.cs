using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.UI.BattleScene.Buttons;
using BattleCruisers.UI.BattleScene.BuildMenus;
using System.Collections.ObjectModel;

namespace BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.UI.BattleScene.BuildMenus
{
    public interface IPvPBuildablesMenu : IMenu
    {
        ReadOnlyCollection<IPvPBuildableButton> BuildableButtons { get; }
    }
}