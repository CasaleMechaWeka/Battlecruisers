using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using BattleCruisers.Data;
using BattleCruisers.Utils;
using BattleCruisers.Utils.Localisation;
using BattleCruisers.UI.Sound.Players;
using BattleCruisers.Data.Static;

namespace BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.UI.Common.HeckleMessage
{
    public class PvPHeckleMessage : MonoBehaviour
    {
        public Text message;
        public float hideTime = 5f;
        private RectTransform messageFrame;

        public void Initialise(ISingleSoundPlayer soundPlayer)
        {
            Helper.AssertIsNotNull(soundPlayer);
            messageFrame = GetComponent<RectTransform>();
        }

        public void Show(int heckleIndex)
        {
            message.text = LocTableCache.HecklesTable.GetString(StaticData.Heckles[heckleIndex].StringKeyBase);
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
