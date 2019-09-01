using BattleCruisers.Buildables;
using BattleCruisers.UI.BattleScene.Buttons;
using BattleCruisers.UI.BattleScene.Buttons.Filters;
using BattleCruisers.UI.BattleScene.Manager;
using BattleCruisers.UI.BattleScene.Presentables;
using BattleCruisers.UI.Filters;
using BattleCruisers.UI.Sound;
using BattleCruisers.Utils;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using UnityEngine.Assertions;

namespace BattleCruisers.UI.BattleScene.BuildMenus
{
    public abstract class BuildablesMenuController<TButton, TBuildable> : PresentableController, IBuildablesMenu
        where TButton : BuildableButtonController
        where TBuildable : class, IBuildable
	{
        protected IUIManager _uiManager;
        protected IBroadcastingFilter<IBuildable> _shouldBeEnabledFilter;

        public ReadOnlyCollection<IBuildableButton> BuildableButtons { get; private set; }

		public virtual void Initialise(
            ISoundPlayer soundPlayer,
            IUIManager uiManager,
            IButtonVisibilityFilters buttonVisibilityFilters,
            IList<IBuildableWrapper<TBuildable>> buildables)
        {
            base.Initialise(soundPlayer);

            Helper.AssertIsNotNull(uiManager, buttonVisibilityFilters, buildables);

            _uiManager = uiManager;
            _shouldBeEnabledFilter = buttonVisibilityFilters.BuildableButtonVisibilityFilter;

            IList<TButton> buildableButtons = GetComponentsInChildren<TButton>().ToList();
            Assert.IsTrue(buildables.Count <= buildableButtons.Count, "Buildable count " + buildables.Count + " should be <= button count " + buildableButtons.Count);

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
                    .Select(button => (IBuildableButton)button)
                    .ToList()
                    .AsReadOnly();
        }

        protected abstract void InitialiseBuildableButton(TButton button, IBuildableWrapper<TBuildable> buildableWrapper);
	}
}
