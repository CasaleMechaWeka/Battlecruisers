using BattleCruisers.Data;
using BattleCruisers.Data.Static;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.UI.BattleScene;
using BattleCruisers.UI.Panels;
using BattleCruisers.UI.Sound.Players;
using BattleCruisers.Utils;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;

namespace BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.UI.Common.HeckleMessage
{
    public class PvPHecklePanelController : MonoBehaviour
    {
        public PvPCanvasGroupButton hecklesButton;
        public Sprite closed, opened;
        public SlidingPanel hecklePanel;

        public PvPHeckleButton heckleButtonPrefab;
        public Transform hecklesParent;

        public ISlidingPanel HecklePanel => hecklePanel;

        private ISingleSoundPlayer _soundPlayer;
        private PvPUIManager _puUIManager;
        private bool isOpened;
        public async void Initialise(ISingleSoundPlayer soundPlayer, PvPUIManager uiManager)
        {
            Helper.AssertIsNotNull(soundPlayer, closed, opened, hecklesButton);
            _soundPlayer = soundPlayer;
            _puUIManager = uiManager;

            hecklesButton.gameObject.GetComponent<Image>().sprite = closed;
            isOpened = false;
            hecklesButton.Initialise(_soundPlayer, OnHeckleButtonClicked);
            hecklePanel.Initialise();
            await Task.Delay(10);
            foreach (int i in DataProvider.GameModel.PlayerLoadout.CurrentHeckles)
            {
                PvPHeckleButton heckleButton = Instantiate(heckleButtonPrefab, hecklesParent);
                heckleButton.StaticInitialise(soundPlayer, StaticData.Heckles[i], this);
            }
        }

        public void OnHeckleButtonClicked()
        {
            if (isOpened)
            {
                hecklesButton.gameObject.GetComponent<Image>().sprite = closed;
                isOpened = false;
                HecklePanel.Hide();
            }
            else
            {
                hecklesButton.gameObject.GetComponent<Image>().sprite = opened;
                isOpened = true;
                HecklePanel.Show();
                _puUIManager.HideItemDetails();
            }
        }

        public void Show()
        {
            hecklesButton.gameObject.GetComponent<Image>().sprite = opened;
            isOpened = true;
            HecklePanel.Show();
        }
        public void Hide()
        {
            hecklesButton.gameObject.GetComponent<Image>().sprite = closed;
            isOpened = false;
            HecklePanel.Hide();
        }
    }
}
