using BattleCruisers.Data;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.UI.BattleScene.Manager;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.UI.Panels;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.UI.Sound.Players;
using BattleCruisers.UI;
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
        public PvPSlidingPanel hecklePanel;

        public PvPHeckleButton heckleButtonPrefab;
        public Transform hecklesParent;

        public IPvPSlidingPanel HecklePanel => hecklePanel;

        private IDataProvider _dataProvider;
        private IPvPSingleSoundPlayer _soundPlayer;
        private IPvPUIManager _puUIManager;
        private bool isOpened;
        public async void Initialise(IDataProvider dataProvider, IPvPSingleSoundPlayer soundPlayer, IPvPUIManager uiManager)
        {
            Helper.AssertIsNotNull(dataProvider, soundPlayer, closed, opened, hecklesButton);
            _dataProvider = dataProvider;
            _soundPlayer = soundPlayer;
            _puUIManager = uiManager;

            hecklesButton.gameObject.GetComponent<Image>().sprite = closed;
            isOpened = false;
            hecklesButton.Initialise(_soundPlayer, OnHeckleButtonClicked);
            hecklePanel.Initialise();
            await Task.Delay(10);
            foreach (int i in _dataProvider.GameModel.PlayerLoadout.CurrentHeckles)
            {
                PvPHeckleButton heckleButton = Instantiate(heckleButtonPrefab, hecklesParent);
                heckleButton.StaticInitialise(soundPlayer, dataProvider, _dataProvider.GameModel.Heckles[i], this);
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
