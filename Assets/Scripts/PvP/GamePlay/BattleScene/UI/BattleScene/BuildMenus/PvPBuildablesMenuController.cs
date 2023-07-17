using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Buildables;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.UI.BattleScene.Buttons;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.UI.BattleScene.Buttons.Filters;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.UI.BattleScene.Manager;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.UI.BattleScene.Presentables;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.UI.Filters;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Utils;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using UnityEngine.Assertions;
using UnityEngine;

namespace BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.UI.BattleScene.BuildMenus
{
    public abstract class PvPBuildablesMenuController<TButton, TBuildable> : PvPPresentableController, IPvPBuildablesMenu
        where TButton : PvPBuildableButtonController
        where TBuildable : class, IPvPBuildable
    {
        protected IPvPUIManager _uiManager;
        protected IPvPBroadcastingFilter<IPvPBuildable> _shouldBeEnabledFilter;

        public ReadOnlyCollection<IPvPBuildableButton> BuildableButtons { get; private set; }

        public virtual void Initialise(
            IPvPUIManager uiManager,
            IPvPButtonVisibilityFilters buttonVisibilityFilters,
            IList<IPvPBuildableWrapper<TBuildable>> buildables)
        {
            base.Initialise();

            PvPHelper.AssertIsNotNull(uiManager, buttonVisibilityFilters, buildables);

            _uiManager = uiManager;
            _shouldBeEnabledFilter = buttonVisibilityFilters.BuildableButtonVisibilityFilter;

            IList<TButton> buildableButtons = GetComponentsInChildren<TButton>().ToList();
            // sava added
        //    Assert.IsTrue(buildables.Count <= buildableButtons.Count, "Buildable count " + buildables.Count + " should be <= button count " + buildableButtons.Count);

            for (int i = 0; i < buildableButtons.Count; ++i)
            {
                TButton buildableButton = buildableButtons[i];

                if (i < buildables.Count)
                {
                    // Have buildable for button
                    InitialiseBuildableButton(buildableButton, buildables[i]);
                    AddChildPresentable(buildableButton);
                }
                else
                {
                    // Have no buildable for button (user has not unlocked it yet)
                    Destroy(buildableButton.gameObject);
                }
            }

            BuildableButtons
                = buildableButtons
                    .Where(button => button.Buildable != null)
                    .Select(button => (IPvPBuildableButton)button)
                    .ToList()
                    .AsReadOnly();
        }

        protected abstract void InitialiseBuildableButton(TButton button, IPvPBuildableWrapper<TBuildable> buildableWrapper);
    }
}
