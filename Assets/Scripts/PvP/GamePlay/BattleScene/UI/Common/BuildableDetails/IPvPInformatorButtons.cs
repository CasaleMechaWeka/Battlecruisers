using BattleCruisers.Buildables;

namespace BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.UI.Common.BuildableDetails
{
    public interface IPvPInformatorButtons
    {
        ITarget SelectedItem { set; }
        IPvPButton ToggleDronesButton { get; }
    }
}
