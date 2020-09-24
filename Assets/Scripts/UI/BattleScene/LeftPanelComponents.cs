using BattleCruisers.Tutorial.Highlighting;
using BattleCruisers.UI.BattleScene.BuildMenus;
using BattleCruisers.Utils;
using BattleCruisers.Utils.PlatformAbstractions;

namespace BattleCruisers.UI.BattleScene
{
    public class LeftPanelComponents
    {
        public IHighlightable NumberOfDronesHighlightable { get; }
        public IBuildMenu BuildMenu { get; }
        public IGameObject PopLimitReachedFeedback { get; }

        public LeftPanelComponents(IHighlightable numberOfDronesHighlightable, IBuildMenu buildMenu, IGameObject popLimitReachedFeedback)
        {
            Helper.AssertIsNotNull(numberOfDronesHighlightable, buildMenu, popLimitReachedFeedback);

            NumberOfDronesHighlightable = numberOfDronesHighlightable;
            BuildMenu = buildMenu;
            PopLimitReachedFeedback = popLimitReachedFeedback;
        }
    }
}