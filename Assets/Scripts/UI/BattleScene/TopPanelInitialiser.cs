using BattleCruisers.Cruisers;
using BattleCruisers.Tutorial.Highlighting;
using BattleCruisers.UI.BattleScene.ProgressBars;
using BattleCruisers.UI.Filters;
using BattleCruisers.UI.ScreensScene.TrashScreen;
using BattleCruisers.Utils;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.UI;

namespace BattleCruisers.UI.BattleScene
{
    public class TopPanelInitialiser : MonoBehaviour
    {
        public TrashTalkDataList trashTalkList;
        public Text enemyHealthBarHelpLabel;

        public async Task<TopPanelComponents> InitialiseAsync(
            ICruiser playerCruiser, 
            ICruiser aiCruiser, 
            IBroadcastingFilter helpLabelVisibilityFilter,
            int levelNum)
        {
            Helper.AssertIsNotNull(trashTalkList, enemyHealthBarHelpLabel);
            Helper.AssertIsNotNull(playerCruiser, aiCruiser, helpLabelVisibilityFilter);

            CruiserHealthBarInitialiser playerHealthInitialiser = transform.FindNamedComponent<CruiserHealthBarInitialiser>("PlayerCruiserHealth/Foreground");
            Assert.IsNotNull(playerHealthInitialiser);
            IHighlightable playerCruiserHealthBar = playerHealthInitialiser.Initialise(playerCruiser, helpLabelVisibilityFilter);

            CruiserHealthBarInitialiser aiHealthInitialiser = transform.FindNamedComponent<CruiserHealthBarInitialiser>("AICruiserHealth/Foreground");
            Assert.IsNotNull(aiHealthInitialiser);
            IHighlightable aiCruiserHealthBar = aiHealthInitialiser.Initialise(aiCruiser, helpLabelVisibilityFilter);

            trashTalkList.Initialise();
            ITrashTalkData levelTrashTalkData = await trashTalkList.GetTrashTalkAsync(levelNum);
            Destroy(trashTalkList.gameObject);
            enemyHealthBarHelpLabel.text = levelTrashTalkData.EnemyName.ToUpper();

            return new TopPanelComponents(playerCruiserHealthBar, aiCruiserHealthBar);
        }
    }
}