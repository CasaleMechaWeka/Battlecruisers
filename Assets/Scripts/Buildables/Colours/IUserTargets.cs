using UnityEngine;

namespace BattleCruisers.Buildables.Colours
{
    public static class TargetColours
    {
        public static Color Default = Color.black;
        public static Color Selected = new Color(186f / 255f, 56f / 255f, 32f / 255f);    // Orange
        public static Color Targetted = Color.red;
    }

    public interface IUserTargets
    {
        /// <summary>
        /// The target the user has selected to show in the informator. Ie,
        /// the user wants to know about the target.
        /// </summary>
        ITarget SelectedTarget { set; }

        /// <summary>
        /// The target the user has chosen to be the highest priority target.
        /// Ie, the target all buildables try to attack!
        /// </summary>
        ITarget TargetToAttack { set; }
    }
}