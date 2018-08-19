using BattleCruisers.Buildables;
using BattleCruisers.Data.Models.PrefabKeys;
using BattleCruisers.UI.ScreensScene.LoadoutScreen.Rows.LoadoutItems;
using UnityEngine;

namespace BattleCruisers.UI.ScreensScene.LoadoutScreen.Rows
{
    public abstract class BuildablesRowWrapper<TBuildable, TPrefabKey> : MonoBehaviour
        where TBuildable : class, IBuildable
        where TPrefabKey : class, IPrefabKey
    {
        public LoadoutBuildableItemsRow<TBuildable, TPrefabKey> BuildablesRow { get; protected set; }

        public abstract void Initialise(IItemsRowArgs<TBuildable> args);
    }
}
