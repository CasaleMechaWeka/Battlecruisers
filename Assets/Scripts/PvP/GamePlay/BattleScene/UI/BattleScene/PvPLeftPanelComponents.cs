using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.UI.BattleScene.BuildMenus;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Utils;
using BattleCruisers.Utils.PlatformAbstractions;
using BattleCruisers.Tutorial.Highlighting;

namespace BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.UI.BattleScene
{
    public class PvPLeftPanelComponents
    {
        public IHighlightable NumberOfDronesHighlightable { get; }
        public PvPBuildMenu BuildMenu { get; }
        public IGameObject PopLimitReachedFeedback { get; }

        public PvPLeftPanelComponents(IHighlightable numberOfDronesHighlightable, PvPBuildMenu buildMenu, IGameObject popLimitReachedFeedback)
        {
            PvPHelper.AssertIsNotNull(numberOfDronesHighlightable, buildMenu, popLimitReachedFeedback);

            NumberOfDronesHighlightable = numberOfDronesHighlightable;
            BuildMenu = buildMenu;
            PopLimitReachedFeedback = popLimitReachedFeedback;
        }
    }
}