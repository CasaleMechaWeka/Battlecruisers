using BattleCruisers.Data;
using BattleCruisers.Data.Models;
using BattleCruisers.Data.Settings;
using BattleCruisers.UI;
using BattleCruisers.UI.Sound.Players;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Scenes
{
    public class BountyScreenHandler : MonoBehaviour
    {
        [Header("Shared UI")]
        public GameObject wholeBountySection;

        [Header("Winner UI")]
        public GameObject winnerScreen;
        public Text winnerBountyReward;
        public CanvasGroupButton goToDestructionSceneButtonWinner;

        [Header("Loser UI")]
        public GameObject loserScreen;
        public GameObject loserText;
        public CanvasGroupButton goToDestructionSceneButtonLoser;
        public CanvasGroupButton goToPlaceBountyScreenLoser;

        [Header("Place Bounty UI")]
        public GameObject placeBountyScreen;
        public TMP_InputField bountyInputField;
        public CanvasGroupButton placeBountyButton;
        public CanvasGroupButton bountyPlacedButton;

        private bool _wasVictory;
        private GameModel _localPlayerModel;

        public void Initialise(SingleSoundPlayer soundPlayer)
        {
            _localPlayerModel = DataProvider.GameModel;
            _wasVictory = PvPBattleResult.WasVictory;

            goToDestructionSceneButtonWinner.Initialise(soundPlayer, GoToDestructionScene);
            goToDestructionSceneButtonLoser.Initialise(soundPlayer, GoToDestructionScene);
            goToPlaceBountyScreenLoser.Initialise(soundPlayer, GoToPlaceBountyScreen);
            placeBountyButton.Initialise(soundPlayer, HandleUserInput);

            SetupScreen();
            Debug.LogError("It ran");
        }

        private void SetupScreen()
        {
            if (_wasVictory)
            {
                // Winner view
                winnerScreen.SetActive(true);
                loserScreen.SetActive(false);
            }
            else
            {
                // Loser view
                winnerScreen.SetActive(false);
                loserScreen.SetActive(true);
            }
        }

        private void GoToDestructionScene()
        {
            if (_wasVictory)
            {
                _localPlayerModel.Bounty += 500;
                _localPlayerModel.Bounty += PvPBattleResult.LoserBounty;
            }
            else
            {
                _localPlayerModel.Bounty = 0;
            }

            DataProvider.SaveGame();
            wholeBountySection.SetActive(false);
        }

        private void GoToPlaceBountyScreen()
        {
            goToPlaceBountyScreenLoser.gameObject.SetActive(false);
            goToDestructionSceneButtonLoser.gameObject.SetActive(false);
            loserText.SetActive(false);
            placeBountyScreen.SetActive(true);
        }

        private void HandleUserInput()
        {
            if (!int.TryParse(bountyInputField.text, out int bountyAmount)) return;
            bountyAmount = Mathf.Clamp(bountyAmount, 0, (int)_localPlayerModel.Coins);

            _localPlayerModel.Coins -= bountyAmount;

            //Increment Winner's bounty by bountyAmount

            DataProvider.SaveGame();
            GoToDestructionScene();
        }

    }
}
