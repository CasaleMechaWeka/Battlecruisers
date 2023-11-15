using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.UI.BattleScene.Buttons;
using System.Collections.ObjectModel;

namespace BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.UI.BattleScene.BuildMenus
{
    public interface IPvPBuildablesMenu : IPvPMenu
    {
        ReadOnlyCollection<IPvPBuildableButton> BuildableButtons { get; }
    }
}