using BattleCruisers.Buildables;
using BattleCruisers.Buildables.Units;
using BattleCruisers.UI.ScreensScene.LoadoutScreen.Comparisons;
using BattleCruisers.UI.ScreensScene.LoadoutScreen.ItemDetails;
using BattleCruisers.UI.Sound.Players;
using BattleCruisers.Utils;
using TMPro;
using UnityEngine.Assertions;

namespace BattleCruisers.UI.ScreensScene.LoadoutScreen.Items
{
    public class UnitButtonV2 : ItemButton
    {
        private IBuildableWrapper<IUnit> _unitPrefab;
        public override IComparableItem Item => _unitPrefab.Buildable;
        public TextMeshProUGUI _unitName;

        public void Initialise(
            ISingleSoundPlayer soundPlayer, 
            IItemDetailsManager itemDetailsManager, 
            IComparingItemFamilyTracker comparingItemFamily,
            IBuildableWrapper<IUnit> unitPrefab,
            PrefabKeyName unitKeyName)
        {
            base.Initialise(soundPlayer, itemDetailsManager, comparingItemFamily);

            //_unitName.text = (unitKeyName.ToString()).Replace("Unit_", string.Empty);

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