using BattleCruisers.Buildables;
using BattleCruisers.Buildables.Units;
using BattleCruisers.UI.ScreensScene.LoadoutScreen.Comparisons;
using BattleCruisers.UI.ScreensScene.LoadoutScreen.ItemDetails;
using BattleCruisers.UI.Sound;
using UnityEngine.Assertions;

namespace BattleCruisers.UI.ScreensScene.LoadoutScreen.Items
{
    public class UnitButton : ItemButton
    {
        private IBuildableWrapper<IUnit> _unitPrefab;
        public override IComparableItem Item => _unitPrefab.Buildable;

        public void Initialise(
            ISoundPlayer soundPlayer, 
            IItemDetailsManager itemDetailsManager, 
            IComparingItemFamilyTracker comparingItemFamily,
            IBuildableWrapper<IUnit> unitPrefab)
        {
            base.Initialise(soundPlayer, itemDetailsManager, comparingItemFamily);

            Assert.IsNotNull(unitPrefab);
            _unitPrefab = unitPrefab;
        }

        protected override void OnClicked()
        {
            base.OnClicked();

            if (_comparingFamiltyTracker.ComparingFamily.Value == null)
            {
                _itemDetailsManager.ShowDetails(_unitPrefab.Buildable);
            }
            else
            {
                _itemDetailsManager.CompareWithSelectedItem(_unitPrefab.Buildable);
                _comparingFamiltyTracker.SetComparingFamily(null);
            }
        }
    }
}