using BattleCruisers.Buildables;
using BattleCruisers.UI;

namespace BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.UI.Common.BuildableDetails
{
    public interface IPvPInformatorButtons
    {
        ITarget SelectedItem { set; }
        IButton ToggleDronesButton { get; }
    }
}
