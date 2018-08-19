using BattleCruisers.Buildables.Units;
using BattleCruisers.Data.Models.PrefabKeys;

namespace BattleCruisers.UI.ScreensScene.LoadoutScreen.Rows.LoadoutItems
{
    public class LoadoutUnitItemWrapper : LoadoutItemWrapper<IUnit, UnitKey>
    {
        protected override bool IsItemUnlocked()
        {
            return _gameModel.UnlockedUnits.Contains(_itemKey);
        }
    }
}