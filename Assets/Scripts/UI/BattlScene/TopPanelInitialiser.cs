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

            PlayerCruiserHealthDialInitialiser playerHealthInitialiser = transform.FindNamedComponent<PlayerCruiserHealthDialInitialiser>("PlayerCruiserHealth/Foreground");
            Assert.IsNotNull(playerHealthInitialiser);
            playerHealthInitialiser.Initialise(playerCruiser);

            PlayerCruiserHealthDialInitialiser aiHealthInitialiser = transform.FindNamedComponent<PlayerCruiserHealthDialInitialiser>("AICruiserHealth/Foreground");
            Assert.IsNotNull(aiHealthInitialiser);
            aiHealthInitialiser.Initialise(aiCruiser);
        }
    }
}