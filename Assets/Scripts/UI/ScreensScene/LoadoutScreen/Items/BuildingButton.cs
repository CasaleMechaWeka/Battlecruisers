using BattleCruisers.Buildables;
using BattleCruisers.Buildables.Buildings;
using BattleCruisers.UI.ScreensScene.LoadoutScreen.Comparisons;
using BattleCruisers.UI.ScreensScene.LoadoutScreen.ItemDetails;
using BattleCruisers.UI.Sound;
using UnityEngine.Assertions;

namespace BattleCruisers.UI.ScreensScene.LoadoutScreen.Items
{
    public class BuildingButton : ItemButton
    {
        public IBuildableWrapper<IBuilding> _buildingPrefab;
        public override IComparableItem Item => _buildingPrefab.Buildable;

        public void Initialise(
            ISoundPlayer soundPlayer, 
            IItemDetailsManager itemDetailsManager, 
            IComparingItemFamilyTracker comparingFamiltyTracker,
            IBuildableWrapper<IBuilding> buildingPrefab)
        {
            base.Initialise(soundPlayer, itemDetailsManager, comparingFamiltyTracker);

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