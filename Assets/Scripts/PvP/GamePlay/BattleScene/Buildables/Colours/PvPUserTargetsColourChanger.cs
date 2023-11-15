namespace BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Buildables.Colours
{
    /// <summary>
    /// Changes a targets colour depending on whether is is selected or not.
    /// </summary>
    public class PvPUserTargetsColourChanger : IPvPUserTargets
    {
        private IPvPTarget _selectedTarget;
        public IPvPTarget SelectedTarget
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