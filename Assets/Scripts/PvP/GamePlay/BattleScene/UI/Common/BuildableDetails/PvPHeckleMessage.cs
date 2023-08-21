using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using BattleCruisers.Data;
using BattleCruisers.UI.Sound.Players;
using BattleCruisers.Utils;
using BattleCruisers.Utils.Localisation;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.UI.Sound.Players;

namespace BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.UI.Common.HeckleMessage
{
    public class PvPHeckleMessage : MonoBehaviour
    {
        public Text message;
        public float hideTime = 5f;
        private RectTransform messageFrame;

        private IDataProvider _dataProvider;
        private IPvPSingleSoundPlayer _soundPlayer;
        private ILocTable heckleStrings;
        public  async void Initialise(IDataProvider dataProvider, IPvPSingleSoundPlayer soundPlayer)
        {
            Helper.AssertIsNotNull(dataProvider, soundPlayer);
            _dataProvider = dataProvider;
            _soundPlayer = soundPlayer;
            heckleStrings = await LocTableFactory.Instance.LoadHecklesTableAsync();
            messageFrame = GetComponent<RectTransform>();
        }

        public void Show(int heckleIndex)
        {
            message.text = heckleStrings.GetString(_dataProvider.GameModel.Heckles[heckleIndex].stringKeyBase);
            messageFrame.localScale = Vector3.zero;
            messageFrame.DOScale(Vector3.one, 0.2f);
            Invoke("Hide", hideTime);
        }

        public void Hide()
        {
            messageFrame.DOScale(Vector3.zero, 0.2f);
        }
    }
}
