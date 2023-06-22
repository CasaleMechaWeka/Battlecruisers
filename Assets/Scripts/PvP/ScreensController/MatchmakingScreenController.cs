using System;
using System.Collections;
using System.Collections.Generic;
using BattleCruisers.Data;
using BattleCruisers.Network.Multiplay.Scenes;
using BattleCruisers.Scenes;
using BattleCruisers.UI.Common;
using BattleCruisers.UI.Sound.Players;
using BattleCruisers.Utils;
using UnityEngine;
using UnityEngine.SceneManagement;
using System.Threading.Tasks;
using BattleCruisers.Network.Multiplay.Gameplay.UI;
using BattleCruisers.Network.Multiplay.Infrastructure;
using BattleCruisers.UI.ScreensScene.TrashScreen;
using BattleCruisers.Utils.Localisation;

namespace BattleCruisers.UI.ScreensScene.Multiplay.ArenaScreen
{
    public class MatchmakingScreenController : ScreenController
    {
        private ISceneNavigator _sceneNavigator;
        private ITrashTalkData _trashTalkData;
        private ILocTable _storyStrings;
        public Animator animator;
        public TrashTalkBubblesController trashTalkBubbles;
        public static MatchmakingScreenController Instance { get; private set; }


        public override void OnPresenting(object activationParameter)
        {

        }
        public void Initialise(IScreensSceneGod matchmakingLoadingSceneGod,
                           ISingleSoundPlayer soundPlayer,
                           IDataProvider dataProvider)
        {
            base.Initialise(matchmakingLoadingSceneGod);
            Helper.AssertIsNotNull(dataProvider);
        }
        void Start()
        {
            Instance = this;
            _sceneNavigator = LandingSceneGod.SceneNavigator;
            DontDestroyOnLoad(gameObject);
        }

        public void SetTraskTalkData(ITrashTalkData trashTalkData, ILocTable commonString, ILocTable storyString)
        {
            _trashTalkData = trashTalkData;
            _commonStrings = commonString;
            _storyStrings = storyString;
        }

        public void FoundCompetitor()
        {
            animator.SetBool("Found", true);
         //   StartCoroutine(iTrashTalk());
        }
        IEnumerator iTrashTalk()
        {
            yield return new WaitForSeconds(2f);
            trashTalkBubbles.gameObject.SetActive(true);
            trashTalkBubbles.Initialise(_trashTalkData, _commonStrings, _storyStrings);
        }

        public void NotFound()
        {
            PopupPanel popupPanel = PopupManager.ShowPopupPanel("Matchmaking Error", "You did not find online competitor." +
                " Please check Internet connection!", true, FailedMatchmaking);
        }

        private void FailedMatchmaking()
        {
            GameObject[] objs = GameObject.FindGameObjectsWithTag("ShouldBeDestroyedOnNonPvP");
            foreach (GameObject obj in objs)
            {
                Destroy(obj);
            }
            _sceneNavigator.SceneLoaded(SceneNames.PvP_BOOT_SCENE);
            _sceneNavigator.GoToScene(SceneNames.SCREENS_SCENE, true);
        }

        public void Destroy()
        {
            Destroy(gameObject);
        }

    }
}

