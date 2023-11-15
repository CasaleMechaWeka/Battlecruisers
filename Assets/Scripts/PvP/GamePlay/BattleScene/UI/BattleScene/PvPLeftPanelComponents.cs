using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Tutorial.Highlighting;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.UI.BattleScene.BuildMenus;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Utils;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Utils.PlatformAbstractions;

namespace BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.UI.BattleScene
{
    public class PvPLeftPanelComponents
    {
        public IPvPHighlightable NumberOfDronesHighlightable { get; }
        public IPvPBuildMenu BuildMenu { get; }
        public IPvPGameObject PopLimitReachedFeedback { get; }

        public PvPLeftPanelComponents(IPvPHighlightable numberOfDronesHighlightable, IPvPBuildMenu buildMenu, IPvPGameObject popLimitReachedFeedback)
        {
            PvPHelper.AssertIsNotNull(numberOfDronesHighlightable, buildMenu, popLimitReachedFeedback);

            NumberOfDronesHighlightable = numberOfDronesHighlightable;
            BuildMenu = buildMenu;
            PopLimitReachedFeedback = popLimitReachedFeedback;
        }
    }
}