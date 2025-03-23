using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using BattleCruisers.Data;
using BattleCruisers.Utils;
using BattleCruisers.Utils.Localisation;
using BattleCruisers.UI.Sound.Players;

namespace BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.UI.Common.HeckleMessage
{
    public class PvPHeckleMessage : MonoBehaviour
    {
        public Text message;
        public float hideTime = 5f;
        private RectTransform messageFrame;

        private IDataProvider _dataProvider;
        private ISingleSoundPlayer _soundPlayer;

        public void Initialise(IDataProvider dataProvider, ISingleSoundPlayer soundPlayer)
        {
            Helper.AssertIsNotNull(dataProvider, soundPlayer);
            _dataProvider = dataProvider;
            _soundPlayer = soundPlayer;
            messageFrame = GetComponent<RectTransform>();
        }

        public void Show(int heckleIndex)
        {
            message.text = LocTableFactory.HecklesTable.GetString(_dataProvider.StaticData.Heckles[heckleIndex].StringKeyBase);
            messageFrame.localScale = Vector3.zero;
            messageFrame.DOScale(Vector3.one * 1.5f, 0.2f);
            Invoke("Hide", hideTime);
        }

        public void Hide()
        {
            messageFrame.DOScale(Vector3.zero, 0.2f);
        }
    }
}
