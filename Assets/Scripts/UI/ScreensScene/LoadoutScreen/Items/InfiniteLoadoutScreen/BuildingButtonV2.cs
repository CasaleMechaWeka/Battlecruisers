using BattleCruisers.Buildables;
using BattleCruisers.Buildables.Buildings;
using BattleCruisers.UI.ScreensScene.LoadoutScreen.Comparisons;
using BattleCruisers.UI.ScreensScene.LoadoutScreen.ItemDetails;
using BattleCruisers.UI.Sound.Players;
using BattleCruisers.Utils;
using TMPro;
using UnityEngine;
using UnityEngine.Assertions;

namespace BattleCruisers.UI.ScreensScene.LoadoutScreen.Items
{
    public class BuildingButtonV2 : ItemButton
    {
        public IBuildableWrapper<IBuilding> _buildingPrefab;
        public override IComparableItem Item => _buildingPrefab.Buildable;
        public TextMeshProUGUI _buildingName;
        private RectTransform _selectedFeedback;

        public void Initialise(
            ISingleSoundPlayer soundPlayer, 
            IItemDetailsManager itemDetailsManager, 
            IComparingItemFamilyTracker comparingFamiltyTracker,
            IBuildableWrapper<IBuilding> buildingPrefab,
            PrefabKeyName buildingKeyName)
        {
            base.Initialise(soundPlayer, itemDetailsManager, comparingFamiltyTracker);
            //_buildingName.text = (buildingKeyName.ToString()).Replace("Building_", string.Empty);
            _selectedFeedback = transform.FindNamedComponent<RectTransform>("SelectedFeedback");
            Assert.IsNotNull(_selectedFeedback);

            Assert.IsNotNull(buildingPrefab);
            _buildingPrefab = buildingPrefab;
        }

        protected override void OnClicked()
        {
            base.OnClicked();

            if (_comparingFamiltyTracker.ComparingFamily.Value == null)
            {
                _itemDetailsManager.ShowDetails(_buildingPrefab.Buildable);
            }
            else
            {
                _itemDetailsManager.CompareWithSelectedItem(_buildingPrefab.Buildable);
                _comparingFamiltyTracker.SetComparingFamily(null);
            }
        }
    }
}