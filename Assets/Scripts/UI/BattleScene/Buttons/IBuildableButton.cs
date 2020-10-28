using BattleCruisers.Buildables;
using BattleCruisers.UI.BattleScene.Presentables;
using UnityEngine;

namespace BattleCruisers.UI.BattleScene.Buttons
{
    public interface IBuildableButton : IButton, IPresentable
    {
        IBuildable Buildable { get; }
        Color Color { set; }

        /// <summary>
        /// For hotkeys.
        /// </summary>
        void TriggerClick();
    }
}
