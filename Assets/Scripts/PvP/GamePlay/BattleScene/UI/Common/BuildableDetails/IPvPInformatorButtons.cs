using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Buildables;

namespace BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.UI.Common.BuildableDetails
{
    public interface IPvPInformatorButtons
    {
        IPvPTarget SelectedItem { set; }
        IPvPButton ToggleDronesButton { get; }
    }
}
