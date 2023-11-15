using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Cruisers;
using BattleCruisers.UI.ScreensScene.ProfileScreen;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

namespace BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene
{
    public class PvPCaptainExoHUDController : MonoBehaviour
    {
        private CaptainExo _leftCaptain, _rightCaptain;
        private Animator _leftAnimator, _rightAnimator;
        public Action DoLeftHappy, DoLeftAngry, DoLeftTaunt;
        public Action DoRightHappy, DoRightAngry, DoRightTaunt;

        public static PvPCaptainExoHUDController Instance;

        private void Start()
        {
            if (Instance == null)
                Instance = this;
        }
        public void Initialize(CaptainExo leftCaptain, CaptainExo rightCaptain)
        {
            _leftCaptain = leftCaptain; _rightCaptain = rightCaptain;
            _leftAnimator = leftCaptain.gameObject.GetComponentInChildren<Animator>();
            _rightAnimator = rightCaptain.gameObject.GetComponentInChildren<Animator>();

            DoLeftAngry += OnDoLeftAngry;
            DoLeftHappy += OnDoLeftHappy;
            DoLeftTaunt += OnDoLeftTaunt;

            DoRightAngry += OnDoRightAngry;
            DoRightHappy += OnDoRightHappy;
            DoRightTaunt += OnDoRightTaunt;
        }

        private void OnDoLeftHappy()
        {
            _leftAnimator.SetTrigger("happy");
        }

        private void OnDoRightHappy()
        {
            _rightAnimator.SetTrigger("happy");
        }
        private void OnDoLeftAngry() 
        {
            _leftAnimator.SetTrigger("angry");
        }
        private void OnDoRightAngry() 
        {
            _rightAnimator.SetTrigger("angry");
        }
        private void OnDoLeftTaunt() 
        {
            _leftAnimator.SetTrigger("taunt");
        }
        private void OnDoRightTaunt() 
        {
            _rightAnimator.SetTrigger("taunt");
        }

        private void OnDestroy()
        {
            DoLeftAngry -= OnDoLeftAngry;
            DoLeftHappy -= OnDoLeftHappy;
            DoLeftTaunt -= OnDoLeftTaunt;

            DoRightAngry -= OnDoRightAngry;
            DoRightHappy -= OnDoRightHappy;
            DoRightTaunt -= OnDoRightTaunt;
        }
    }
}
