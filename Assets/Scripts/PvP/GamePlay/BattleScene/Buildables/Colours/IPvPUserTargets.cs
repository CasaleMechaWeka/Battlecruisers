using UnityEngine;

namespace BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Buildables.Colours
{
    public static class TargetColours
    {
        public static Color Default = Color.black;
        public static Color Selected = new Color(186f / 255f, 56f / 255f, 32f / 255f);    // Orange
    }

    public interface IPvPUserTargets
    {
        /// <summary>
        /// The target the user has selected to show in the informator. Ie,
        /// the user wants to know about the target.
        /// </summary>
        IPvPTarget SelectedTarget { set; }
    }
}