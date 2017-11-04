using BattleCruisers.Buildables;
using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.UI;

namespace BattleCruisers.UI.Common.BuildingDetails.Buttons
{
    public class DeleteButtonController : MonoBehaviour
    {
        private Button _button;
		private BuildableDetailsController _details;

		public IBuildable Buildable { private get; set; }

        public void Initialise(BuildableDetailsController details)
        {
            Assert.IsNotNull(details);
            _details = details;

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
