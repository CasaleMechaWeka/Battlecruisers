using BattleCruisers.Data.Models;
using BattleCruisers.Data.Models.PrefabKeys;
using BattleCruisers.Utils;
using System.Collections.Specialized;

namespace BattleCruisers.UI.ScreensScene.LoadoutScreen.Items
{
    public class BuildingItemContainer : ItemContainer
    {
        public PrefabKeyName buildingKeyName;

        private BuildingKey _key;
        private BuildingKey Key
        {
            get
            {
                if (_key == null)
                {
                    _key = StaticPrefabKeyHelper.GetPrefabKey<BuildingKey>(buildingKeyName);
                }
                return _key;
            }
        }

        protected override bool IsUnlocked(IGameModel gameModel)
        {
            return gameModel.UnlockedBuildings.Contains(Key);
        }

        protected override bool IsNew(IGameModel gameModel)
        {
            return gameModel.NewBuildings.Items.Contains(Key);
        }

        protected override void SetupNewMarkVisibilityCallback(IGameModel gameModel)
        {
            gameModel.NewBuildings.Items.Parse<INotifyCollectionChanged>().CollectionChanged += (sender, e) => UpdateNewItemMarkVisibility();
        }

        protected override void MakeOld(IGameModel gameModel)
        {
            gameModel.NewBuildings.RemoveItem(Key);
        }
    }
}