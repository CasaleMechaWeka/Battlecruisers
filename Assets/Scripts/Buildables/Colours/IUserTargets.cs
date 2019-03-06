namespace BattleCruisers.Buildables.Colours
{
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