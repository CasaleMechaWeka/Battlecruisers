using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.UI.Sound.Players;
using System;
using UnityEngine;
using UnityEngine.Assertions;

namespace BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.UI
{
    public class PvPCanvasGroupButton : PvPElementWithClickSound
    {
        private CanvasGroup _canvasGroup;
        protected override CanvasGroup CanvasGroup => _canvasGroup;

        public override void Initialise(
            IPvPSingleSoundPlayer soundPlayer,
            Action clickAction = null,
            IPvPDismissableEmitter parent = null)
        {
            base.Initialise(soundPlayer, clickAction, parent);

            _canvasGroup = GetComponent<CanvasGroup>();
            Assert.IsNotNull(_canvasGroup);
        }
    }
}