using BattleCruisers.Buildables;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.UI.BattleScene.BuildMenus;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Utils;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Utils.PlatformAbstractions;
using BattleCruisers.Tutorial.Highlighting;

namespace BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.UI.BattleScene
{
    public class PvPLeftPanelComponents
    {
        public IHighlightable NumberOfDronesHighlightable { get; }
        public IPvPBuildMenu BuildMenu { get; }
        public IPvPGameObject PopLimitReachedFeedback { get; }

        public PvPLeftPanelComponents(IHighlightable numberOfDronesHighlightable, IPvPBuildMenu buildMenu, IPvPGameObject popLimitReachedFeedback)
        {
            PvPHelper.AssertIsNotNull(numberOfDronesHighlightable, buildMenu, popLimitReachedFeedback);

            NumberOfDronesHighlightable = numberOfDronesHighlightable;
            BuildMenu = buildMenu;
            PopLimitReachedFeedback = popLimitReachedFeedback;
        }
    }
}