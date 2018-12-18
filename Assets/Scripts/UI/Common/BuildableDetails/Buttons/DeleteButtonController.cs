using BattleCruisers.Buildables;
using BattleCruisers.UI.BattleScene.Manager;
using BattleCruisers.Utils;
using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.UI;

namespace BattleCruisers.UI.Common.BuildableDetails.Buttons
{
    public class DeleteButtonController : MonoBehaviour
    {
        private Button _button;
        private IUIManager _uiManager;
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

        public void Initialise(IUIManager uiManager, IFilter<ITarget> buttonVisibilityFilter)
        {
            Helper.AssertIsNotNull(uiManager, buttonVisibilityFilter);

            _uiManager = uiManager;
            _buttonVisibilityFilter = buttonVisibilityFilter;

            _button = GetComponent<Button>();
            Assert.IsNotNull(_button);
            _button.onClick.AddListener(DeleteBuildable);
        }

        private void DeleteBuildable()
        {
            Buildable.InitiateDelete();
            _uiManager.HideItemDetails();
        }
    }
}
