using BattleCruisers.Data.Models;
using BattleCruisers.Data.Models.PrefabKeys;
using BattleCruisers.Utils;

namespace BattleCruisers.UI.ScreensScene.LoadoutScreen.Items
{
    public class UnitItemContainer : ItemContainer
    {
        public PrefabKeyName unitKeyName;

        private UnitKey _key;
        private UnitKey Key
        {
            get
            {
                if (_key == null)
                {
                    _key = StaticPrefabKeyHelper.GetPrefabKey<UnitKey>(unitKeyName);
                }
                return _key;
            }
        }

        protected override bool IsUnlocked(IGameModel gameModel)
        {
            return gameModel.UnlockedUnits.Contains(Key);
        }

        protected override bool IsNew(IGameModel gameModel)
        {
            return gameModel.NewUnits.Items.Contains(Key);
        }

        protected override void MakeOld(IGameModel gameModel)
        {
            gameModel.NewUnits.RemoveItem(Key);
        }
    }
}