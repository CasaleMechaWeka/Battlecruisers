using BattleCruisers.Cruisers;
using BattleCruisers.UI.BattleScene.ProgressBars;
using BattleCruisers.Utils;
using UnityEngine;
using UnityEngine.Assertions;

namespace BattleCruisers.UI.BattleScene
{
    public class TopPanelInitialiser : MonoBehaviour
    {
        // FELIX  Return health bars :)
        public void Initialise(ICruiser playerCruiser, ICruiser aiCruiser)
        {
            Helper.AssertIsNotNull(playerCruiser, aiCruiser);

            PlayerCruiserHealthDialInitialiser dialInitialiser = GetComponentInChildren<PlayerCruiserHealthDialInitialiser>();
            Assert.IsNotNull(dialInitialiser);
            dialInitialiser.Initialise(playerCruiser);

            // FELIX  Initialise aiCruiser health bar :)
        }
    }
}