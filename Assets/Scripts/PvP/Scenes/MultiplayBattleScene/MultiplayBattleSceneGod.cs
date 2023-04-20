using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene
{
    public class MultiplayBattleSceneGod : MonoBehaviour
    {
        // Start is called before the first frame update
        void Start()
        {
            SayHello();
        }

        // Update is called once per frame
        void Update()
        {

        }

        public void SayHello()
        {
            Debug.Log("Say Hello!");
        }
    }
}

