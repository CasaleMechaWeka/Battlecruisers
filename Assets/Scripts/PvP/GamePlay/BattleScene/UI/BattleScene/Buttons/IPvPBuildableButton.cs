using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Buildables;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.UI.BattleScene.Presentables;
using UnityEngine;

namespace BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.UI.BattleScene.Buttons
{
    public interface IPvPBuildableButton : IPvPButton, IPvPPresentable
    {
        IPvPBuildable Buildable { get; }
        Color Color { set; }

        /// <summary>
        /// For hotkeys.
        /// </summary>
        void TriggerClick();
    }
}
