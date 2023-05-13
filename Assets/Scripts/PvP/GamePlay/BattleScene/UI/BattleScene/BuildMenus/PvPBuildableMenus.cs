using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Buildables;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.UI.BattleScene.Buttons.Filters;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.UI.BattleScene.Manager;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.UI.Sound.Players;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Utils;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Utils.Sorting;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using UnityEngine;
using UnityEngine.Assertions;

namespace BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.UI.BattleScene.BuildMenus
{
    public abstract class PvPBuildableMenus<TBuildable, TCategories, TMenu> : MonoBehaviour, IPvPBuildableMenus<TCategories>
        where TBuildable : class, IPvPBuildable
        where TMenu : MonoBehaviour, IPvPBuildablesMenu
    {
        private IDictionary<TCategories, IPvPBuildablesMenu> _buildableCategoryToMenus;

        public IReadOnlyCollection<IPvPBuildablesMenu> Menus { get; private set; }

        public void Initialise(
            IDictionary<TCategories, IList<IPvPBuildableWrapper<TBuildable>>> buildables,
            IPvPUIManager uiManager,
            IPvPButtonVisibilityFilters buttonVisibilityFilters,
            IPvPBuildableSorter<TBuildable> buildableSorter,
            IPvPSingleSoundPlayer soundPlayer)
        {
            PvPHelper.AssertIsNotNull(buildables, uiManager, buttonVisibilityFilters, buildableSorter, soundPlayer);

            IList<TMenu> buildableMenus = GetComponentsInChildren<TMenu>().ToList();


            //Debug.Log(buildables.Count);
            Assert.AreEqual(buildables.Count, buildableMenus.Count);

            _buildableCategoryToMenus = new Dictionary<TCategories, IPvPBuildablesMenu>();

            int i = 0;

            foreach (KeyValuePair<TCategories, IList<IPvPBuildableWrapper<TBuildable>>> pair in buildables)
            {
                TMenu buildableMenu = buildableMenus[i];

                if (pair.Value.Count != 0)
                {
                    IList<IPvPBuildableWrapper<TBuildable>> sortedBuildables = buildableSorter.Sort(pair.Value);
                    InitialiseMenu(soundPlayer, buildableMenu, uiManager, buttonVisibilityFilters, sortedBuildables);
                    _buildableCategoryToMenus.Add(pair.Key, buildableMenu);
                    buildableMenu.IsVisible = false;
                }
                else
                {
                    Destroy(buildableMenu.gameObject);
                }

                i++;
            }

            Menus = new ReadOnlyCollection<IPvPBuildablesMenu>(_buildableCategoryToMenus.Values.ToList());
        }

        protected abstract void InitialiseMenu(
            IPvPSingleSoundPlayer soundPlayer,
            TMenu menu,
            IPvPUIManager uiManager,
            IPvPButtonVisibilityFilters buttonVisibilityFilters,
            IList<IPvPBuildableWrapper<TBuildable>> buildables);

        public IPvPBuildablesMenu GetBuildablesMenu(TCategories buildableCategory)
        {
            Assert.IsTrue(_buildableCategoryToMenus.ContainsKey(buildableCategory));
            return _buildableCategoryToMenus[buildableCategory];
        }
    }
}