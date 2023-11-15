using BattleCruisers.Data;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.UI.Sound.Players;
using BattleCruisers.UI.ScreensScene.ShopScreen;
using BattleCruisers.Utils;
using BattleCruisers.Utils.Localisation;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

namespace BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.UI.Common.HeckleMessage
{
    public class PvPHeckleButton : MonoBehaviour
    {
        public PvPCanvasGroupButton heckleButton;
        public CanvasGroup background;
        public TextMeshProUGUI message;

        private IHeckleData _heckleData;
        private IPvPSingleSoundPlayer _soundPlayer;
        private IDataProvider _dataProvider;
        private ILocTable hecklesStrings;
        private PvPHecklePanelController _panelController;
        public async void StaticInitialise(IPvPSingleSoundPlayer soundPlayer, IDataProvider dataProvider, IHeckleData heckleData, PvPHecklePanelController panelController)
        {
            Helper.AssertIsNotNull(soundPlayer, dataProvider, heckleData, panelController);
            _soundPlayer = soundPlayer;
            _dataProvider = dataProvider;
            _heckleData = heckleData;
            _panelController = panelController;
            hecklesStrings = await LocTableFactory.Instance.LoadHecklesTableAsync();
            heckleButton.Initialise(_soundPlayer, SendHeckleMessage);
            message.text = hecklesStrings.GetString(heckleData.StringKeyBase);
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
