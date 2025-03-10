using BattleCruisers.UI.BattleScene.Buttons;
using System.Collections.ObjectModel;

namespace BattleCruisers.UI.BattleScene.BuildMenus
{
    public interface IBuildablesMenu : IMenu
    {
        ReadOnlyCollection<IBuildableButton> BuildableButtons { get; }
    }
}