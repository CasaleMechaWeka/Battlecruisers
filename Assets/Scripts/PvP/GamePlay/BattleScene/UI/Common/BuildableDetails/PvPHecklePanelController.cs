using BattleCruisers.Data;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.UI.Panels;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.UI.Sound.Players;
using BattleCruisers.UI;
using BattleCruisers.UI.Sound.Players;
using BattleCruisers.Utils;
using UnityEngine;
using UnityEngine.UI;

namespace BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.UI.Common.HeckleMessage
{
    public class PvPHecklePanelController : MonoBehaviour
    {
        public PvPCanvasGroupButton hecklesButton;
        public Sprite closed, opened;
        public PvPSlidingPanel hecklePanel;
        public IPvPSlidingPanel HecklePanel => hecklePanel;

        private IDataProvider _dataProvider;
        private IPvPSingleSoundPlayer _soundPlayer;

        private bool isOpened;
        public void Initialise(IDataProvider dataProvider, IPvPSingleSoundPlayer soundPlayer)
        {
            Helper.AssertIsNotNull(dataProvider, soundPlayer, closed, opened, hecklesButton);
            _dataProvider = dataProvider;
            _soundPlayer = soundPlayer;

            hecklesButton.gameObject.GetComponent<Image>().sprite = closed;
            isOpened = false;
            hecklesButton.Initialise(_soundPlayer, OnHeckleButtonClicked);
            hecklePanel.Initialise();
        }

        public void OnHeckleButtonClicked()
        {
            if(isOpened)
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
            }
        }
    }
}
