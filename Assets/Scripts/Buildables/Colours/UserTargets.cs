namespace BattleCruisers.Buildables.Colours
{
    // FELIX  Test :)
    public class UserTargets : IUserTargets
    {
        // The user chosen target colour trumps the selected target colour.
        // Hence, do NOT set the SelectedColor if the TargettedColour is
        // already set.
        private ITarget _selectedTarget;
        public ITarget SelectedTarget
        {
            set
            {
                if (_selectedTarget != null
                    && !ReferenceEquals(_targetToAttack, _selectedTarget))
                {
                    _selectedTarget.Color = TargetColours.Default;
                }

                _selectedTarget = value;

                if (_selectedTarget != null
                    && !ReferenceEquals(_targetToAttack, _selectedTarget))
                {
                    _selectedTarget.Color = TargetColours.Selected;
                }
            }
        }

        private ITarget _targetToAttack;
        public ITarget TargetToAttack
        {
            set
            {
                if (_targetToAttack != null)
                {
                    // When the user clears their chosen target, the target may still be selected.
                    // In this case apply the SelectedColor instead of the DefaultColor.
                    _targetToAttack.Color = ReferenceEquals(_targetToAttack, _selectedTarget) ? TargetColours.Selected : TargetColours.Default;
                }

                _targetToAttack = value;

                if (_targetToAttack != null)
                {
                    _targetToAttack.Color = TargetColours.Targetted;
                }
            }
        }
    }
}