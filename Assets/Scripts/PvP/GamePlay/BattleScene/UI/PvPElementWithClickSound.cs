using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Data.Static;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.UI.Sound;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.UI.Sound.Players;
using System;
using UnityEngine.Assertions;
using UnityEngine;

namespace BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.UI
{
    public class PvPElementWithClickSound : PvPClickableTogglable
    {
        private Action _clickAction;
        private IPvPDismissableEmitter _parent;
        protected IPvPSingleSoundPlayer _soundPlayer;
        protected virtual IPvPSoundKey ClickSound => PvPSoundKeys.UI.Click;

        public virtual void Initialise(
            IPvPSingleSoundPlayer soundPlayer,
            Action clickAction = null,
            IPvPDismissableEmitter parent = null)
        {
            base.Initialise();
            Assert.IsNotNull(soundPlayer);

            _soundPlayer = soundPlayer;
            _clickAction = clickAction;
            _parent = parent;

            if (_parent != null)
            {
                _parent.Dismissed += Parent_Dismissed;
            }
        }

        private void Parent_Dismissed(object sender, EventArgs e)
        {
            Reset();
        }

        protected override void OnClicked()
        {
            if (ClickSound != null)
            {
                if (_soundPlayer == null)
                    Debug.Log("Sound player is null");
                _soundPlayer.PlaySoundAsync(ClickSound);
            }

            _clickAction?.Invoke();
        }

        protected virtual void DestroySelf()
        {
            if (_parent != null)
            {
                _parent.Dismissed -= Parent_Dismissed;
            }
            Destroy(gameObject);
        }
    }
}