using BattleCruisers.Buildables;
using BattleCruisers.Data.Models.PrefabKeys;
using BattleCruisers.UI.ScreensScene.LoadoutScreen.Rows;
using BattleCruisers.Utils;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace BattleCruisers.UI.ScreensScene.LoadoutScreen
{
    public abstract class BuildableSection<TBuildable, TPrefabKey> : MonoBehaviour
        where TBuildable : class, IBuildable
        where TPrefabKey : class, IPrefabKey
    {
        private IList<BuildablesRowWrapper<TBuildable, TPrefabKey>> _buildablesRow;

        protected abstract ItemType ItemType { get; }

        public void Initialise(ItemsRowArgs<TBuildable> args, IItemStateManager itemStateManager)
        {
            Helper.AssertIsNotNull(args, itemStateManager);

            _buildablesRow = GetComponentsInChildren<BuildablesRowWrapper<TBuildable, TPrefabKey>>().ToList();

            foreach (BuildablesRowWrapper<TBuildable, TPrefabKey> buildableRow in _buildablesRow)
            {
                buildableRow.Initialise(args);
                itemStateManager.AddItem(buildableRow.BuildablesRow, ItemType);
            }
        }

        public void OnPresented()
        {
            foreach (BuildablesRowWrapper<TBuildable, TPrefabKey> buildablesRow in _buildablesRow)
            {
                buildablesRow.BuildablesRow.RefreshLockedStatus();
            }
        }
    }
}