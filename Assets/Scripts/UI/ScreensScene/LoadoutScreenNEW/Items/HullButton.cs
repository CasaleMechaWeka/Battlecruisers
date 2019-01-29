using BattleCruisers.Cruisers;
using BattleCruisers.UI.ScreensScene.LoadoutScreenNEW.Comparisons;
using BattleCruisers.UI.ScreensScene.LoadoutScreenNEW.ItemDetails;
using UnityEngine.Assertions;
using UnityEngine.EventSystems;

namespace BattleCruisers.UI.ScreensScene.LoadoutScreenNEW
{
    public class HullButton : ItemButton
    {
        public Cruiser cruiser;

        public override void Initialise(IItemDetailsManager itemDetailsManager, IComparingItemFamilyTracker comparingFamiltyTracker)
        {
            base.Initialise(itemDetailsManager, comparingFamiltyTracker);

            Assert.IsNotNull(cruiser);
            cruiser.StaticInitialise();
        }

        public override void OnPointerClick(PointerEventData eventData)
        {
            if (_comparingFamiltyTracker.ComparingFamily.Value == null)
            {
                _itemDetailsManager.ShowDetails(cruiser);
            }
            else
            {
                _itemDetailsManager.CompareWithSelectedItem(cruiser);
                _comparingFamiltyTracker.SetComparingFamily(null);
            }
        }
    }
}