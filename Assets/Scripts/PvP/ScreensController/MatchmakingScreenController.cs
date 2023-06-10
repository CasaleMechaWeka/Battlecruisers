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


namespace BattleCruisers.UI.ScreensScene.Multiplay.ArenaScreen
{
    public class MatchmakingScreenController : ScreenController
    {

        Animator animator;

        // public static MatchmakingScreenController Instance { get; private set; }

        public bool started = false;
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
            animator = GetComponent<Animator>();
            started = false;
            // Instance = this;
        }

        public void StartMatchmaking()
        {
            if (started)
                return;
            started = true;
            animator.SetBool("Matchmaking", started);
        }
        public void ResetMatchmakingAnimation()
        {
            started = false;
            animator.SetBool("Matchmaking", started);
        }

    }
}

