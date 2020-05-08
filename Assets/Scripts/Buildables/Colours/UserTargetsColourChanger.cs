namespace BattleCruisers.Buildables.Colours
{
    /// <summary>
    /// Changes a targets colour depending on whether is is selected or not.
    /// </summary>
    public class UserTargetsColourChanger : IUserTargets
    {
        private ITarget _selectedTarget;
        public ITarget SelectedTarget
        {
            set
            {
                if (_selectedTarget != null)
                {
                    _selectedTarget.Color = TargetColours.Default;
                }

                _selectedTarget = value;

                if (_selectedTarget != null)
                {
                    _selectedTarget.Color = TargetColours.Selected;
                }
            }
        }
    }
}