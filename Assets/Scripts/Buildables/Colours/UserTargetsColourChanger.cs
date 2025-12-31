using UnityEngine;

namespace BattleCruisers.Buildables.Colours
{
    /// <summary>
    /// Changes a targets colour depending on whether is is selected or not.
    /// </summary>
    public class UserTargetsColourChanger
    {
        private ITarget _selectedTarget;
        private Color selected = new Color(186f / 255f, 56f / 255f, 32f / 255f);
        public ITarget SelectedTarget
        {
            set
            {
                if (_selectedTarget != null)
                {
                    _selectedTarget.Color = Color.black;
                }

                _selectedTarget = value;

                if (_selectedTarget != null)
                {
                    _selectedTarget.Color = selected;
                }
            }
        }
    }
}