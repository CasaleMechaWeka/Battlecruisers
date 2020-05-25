using UnityEngine;
using UnityEngine.Assertions;

namespace BattleCruisers.Utils.Debugging
{
    public class KeyboardCheater : MonoBehaviour
    {
        public Cheater cheater;

        void Start()
        {
            Assert.IsNotNull(cheater);

            if (!Debug.isDebugBuild)
            {
                Destroy(gameObject);
            }
        }

        void Update()
        {
            // W = Win
            if (Input.GetKeyUp(KeyCode.W))
            {
                cheater.Win();
            }
            // L = Loss
            else if (Input.GetKeyUp(KeyCode.L))
            {
                cheater.Lose();
            }
            // B = Builders
            else if (Input.GetKeyUp(KeyCode.B))
            {
                cheater.AddBuilders();
            }
            // T = Toggle UI
            else if (Input.GetKeyUp(KeyCode.T))
            {
                cheater.ToggleUI();
            }
            // N = Nuke
            else if (Input.GetKeyUp(KeyCode.N))
            {
                cheater.ShowNuke();
            }
            // P = Pause
            else if (Input.GetKeyUp(KeyCode.P))
            {
                cheater.TogglePause();
            }
        }
    }
}