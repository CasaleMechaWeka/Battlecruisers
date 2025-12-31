using BattleCruisers.Data;
using BattleCruisers.UI.ScreensScene.ShopScreen;
using BattleCruisers.Utils;
using BattleCruisers.Utils.Localisation;
using UnityEngine;
using TMPro;
using BattleCruisers.UI.Sound.Players;

namespace BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.UI.Common.HeckleMessage
{
    public class PvPHeckleButton : MonoBehaviour
    {
        public PvPCanvasGroupButton heckleButton;
        public CanvasGroup background;
        public TextMeshProUGUI message;

        private HeckleData _heckleData;
        private SingleSoundPlayer _soundPlayer;
        private PvPHecklePanelController _panelController;
        public void StaticInitialise(SingleSoundPlayer soundPlayer, HeckleData heckleData, PvPHecklePanelController panelController)
        {
            Helper.AssertIsNotNull(soundPlayer, heckleData, panelController);
            _soundPlayer = soundPlayer;
            _heckleData = heckleData;
            _panelController = panelController;
            heckleButton.Initialise(_soundPlayer, SendHeckleMessage);
            message.text = LocTableCache.HecklesTable.GetString(heckleData.StringKeyBase);
        }

        public void SendHeckleMessage()
        {
            PvPHeckleMessageManager.Instance.SendHeckle(_heckleData.Index);
            heckleButton.Enabled = false;
            background.alpha = 0.4f;
            _panelController.OnHeckleButtonClicked();
        }
    }
}
