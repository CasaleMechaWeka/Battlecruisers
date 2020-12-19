using UnityEngine;
using UnityEngine.Assertions;

namespace BattleCruisers.Utils.Debugging
{
    public class KeyboardCheater : CheaterBase
    {
        public Cheater cheater;

        void Start()
        {
            Assert.IsNotNull(cheater);
        }

        void Update()
        {
            if (Input.GetKeyUp(KeyCode.F4))
            {
                cheater.ToggleUI();
                cheater.ToggleCursor();
            }

            //// W = Win
            //if (Input.GetKeyUp(KeyCode.W))
            //{
            //    cheater.Win();
            //}
            //// L = Loss
            //else if (Input.GetKeyUp(KeyCode.L))
            //{
            //    cheater.Lose();
            //}
            //// B = Builders
            //else if (Input.GetKeyUp(KeyCode.B))
            //{
            //    cheater.AddBuilders();
            //}
            //// T = Toggle UI
            //else if (Input.GetKeyUp(KeyCode.T))
            //{
            //    cheater.ToggleUI();
            //    cheater.ToggleCursor();
            //}
            //// N = Nuke
            //else if (Input.GetKeyUp(KeyCode.N))
            //{
            //    cheater.ShowNuke();
            //}
            //// P = Pause
            //else if (Input.GetKeyUp(KeyCode.P))
            //{
            //    cheater.TogglePause();
            //}
            //// 1 = Normal build speed
            //else if (Input.GetKeyUp(KeyCode.Alpha1))
            //{
            //    cheater.SetSpeedNormal();
            //}
            //// 2 = Fast build speed (50)
            //else if (Input.GetKeyUp(KeyCode.Alpha2))
            //{
            //    cheater.SetSpeedFast();
            //}
            //// 3 = Very fast build speed (500)
            //else if (Input.GetKeyUp(KeyCode.Alpha3))
            //{
            //    cheater.SetSpeedVeryFast();
            //}
        }
    }
}