using BattleCruisers.Cruisers;
using BattleCruisers.Tutorial.Highlighting;
using BattleCruisers.UI.BattleScene.ProgressBars;
using BattleCruisers.UI.Filters;
using BattleCruisers.UI.ScreensScene.TrashScreen;
using BattleCruisers.Utils;
using BattleCruisers.Utils.Fetchers;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.UI;

namespace BattleCruisers.UI.BattleScene
{
    public class TopPanelInitialiser : MonoBehaviour
    {
        public Text enemyHealthBarHelpLabel;

        public async Task<TopPanelComponents> InitialiseAsync(
            ICruiser playerCruiser, 
            ICruiser aiCruiser, 
            IBroadcastingFilter helpLabelVisibilityFilter,
            int levelNum,
            IPrefabFetcher prefabFetcher)
        {
            Assert.IsNotNull(enemyHealthBarHelpLabel);
            Helper.AssertIsNotNull(playerCruiser, aiCruiser, helpLabelVisibilityFilter, prefabFetcher);

            CruiserHealthBarInitialiser playerHealthInitialiser = transform.FindNamedComponent<CruiserHealthBarInitialiser>("PlayerCruiserHealth/Foreground");
            Assert.IsNotNull(playerHealthInitialiser);
            IHighlightable playerCruiserHealthBar = playerHealthInitialiser.Initialise(playerCruiser, helpLabelVisibilityFilter);

            CruiserHealthBarInitialiser aiHealthInitialiser = transform.FindNamedComponent<CruiserHealthBarInitialiser>("AICruiserHealth/Foreground");
            Assert.IsNotNull(aiHealthInitialiser);
            IHighlightable aiCruiserHealthBar = aiHealthInitialiser.Initialise(aiCruiser, helpLabelVisibilityFilter);

            ITrashTalkProvider trashTalkProvider = new TrashTalkProvider(prefabFetcher);
            ITrashTalkData levelTrashTalkData = await trashTalkProvider.GetTrashTalkAsync(levelNum);
            enemyHealthBarHelpLabel.text = levelTrashTalkData.EnemyName.ToUpper();

            return new TopPanelComponents(playerCruiserHealthBar, aiCruiserHealthBar);
        }
    }
}