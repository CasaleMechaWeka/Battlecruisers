using BattleCruisers.Cruisers;
using BattleCruisers.UI.ScreensScene.LoadoutScreenNEW.ItemDetails;
using UnityEngine.Assertions;

namespace BattleCruisers.UI.ScreensScene.LoadoutScreenNEW
{
    public class HullButton : ItemButton
    {
        public Cruiser cruiser;

        public override void Initialise(IItemDetailsDisplayer itemDetailsDisplayer)
        {
            base.Initialise(itemDetailsDisplayer);

            Assert.IsNotNull(cruiser);
            cruiser.StaticInitialise();
        }

        protected override void ShowItemDetails()
        {
            _itemDetailsDisplayer.ShowDetails(cruiser);
        }
    }
}