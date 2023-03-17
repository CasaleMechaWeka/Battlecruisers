using BattleCruisers.Network.Multiplay.Gameplay.GameState;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace BattleCruisers.Network.Multiplay.Scenes
{
    public class MatchmakingScreensSceneGod : GameStateBehaviour
    {

        public override GameState ActiveState { get { return GameState.MatchmakingScreenScene; } }



        protected override void Awake()
        {
            base.Awake();
        }
        // Start is called before the first frame update
        void Start()
        {

        }

        // Update is called once per frame
        void Update()
        {

        }
    }
}

