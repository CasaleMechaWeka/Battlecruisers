using BattleCruisers.Buildables;
using BattleCruisers.Utils;
using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.UI;

namespace BattleCruisers.UI.Common.BuildableDetails.Buttons
{
    // FELIX  Handle showing/hiding this button like ChooseTargetButton, 
    // don't need to pass "allowDelete" to IBuildableDetails :)
    public class DeleteButtonController : MonoBehaviour
    {
        private Button _button;
        private IHidable _details;
        private IFilter<ITarget> _buttonVisibilityFilter;

        private IBuildable _buildable;
        public IBuildable Buildable
        {
            private get { return _buildable; }
            set
            {
                _buildable = value;
                gameObject.SetActive(_buttonVisibilityFilter.IsMatch(_buildable));
            }
        }

        public void Initialise(IHidable details, IFilter<ITarget> buttonVisibilityFilter)
        {
            Helper.AssertIsNotNull(details, buttonVisibilityFilter);

            _details = details;
            _buttonVisibilityFilter = buttonVisibilityFilter;

            _button = GetComponent<Button>();
            Assert.IsNotNull(_button);
            _button.onClick.AddListener(DeleteBuildable);
        }

        private void DeleteBuildable()
        {
            Buildable.InitiateDelete();
            _details.Hide();
        }
    }
}
