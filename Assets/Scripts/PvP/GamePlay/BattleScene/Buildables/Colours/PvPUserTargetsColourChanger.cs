using BattleCruisers.Buildables;
using BattleCruisers.Buildables.Colours;

namespace BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Buildables.Colours
{
    /// <summary>
    /// Changes a targets colour depending on whether is is selected or not.
    /// </summary>
    public class PvPUserTargetsColourChanger : IUserTargets
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