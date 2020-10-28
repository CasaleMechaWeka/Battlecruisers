using BattleCruisers.Buildables;
using UnityEngine;

namespace BattleCruisers.UI.BattleScene.Buttons
{
    public interface IBuildableButton : IButton
    {
        IBuildable Buildable { get; }
        Color Color { set; }

        /// <summary>
        /// For hotkeys.
        /// </summary>
        void TriggerClick();
    }
}
