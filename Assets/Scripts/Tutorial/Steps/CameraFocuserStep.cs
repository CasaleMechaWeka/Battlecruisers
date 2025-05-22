using System;
using BattleCruisers.UI.BattleScene.Navigation;
using UnityEngine.Assertions;

namespace BattleCruisers.Tutorial.Steps
{
    public enum CameraFocuserTarget
    {
        PlayerCruiser, AICruiser, AICruiserNavalFactory, MidLeft, Overview
    }

    public class CameraFocuserStep : TutorialStep
    {
        private readonly ICameraFocuser _cameraFocuser;
        private readonly CameraFocuserTarget _target;

        public CameraFocuserStep(TutorialStepArgs args, ICameraFocuser cameraFocuser, CameraFocuserTarget target)
            : base(args)
        {
            Assert.IsNotNull(cameraFocuser);

            _cameraFocuser = cameraFocuser;
            _target = target;
        }

        public override void Start(Action completionCallback)
        {
            base.Start(completionCallback);

            switch (_target)
            {
                case CameraFocuserTarget.PlayerCruiser:
                    _cameraFocuser.FocusOnLeftCruiser();
                    break;

                case CameraFocuserTarget.AICruiser:
                    _cameraFocuser.FocusOnRightCruiser();
                    break;

                case CameraFocuserTarget.AICruiserNavalFactory:
                    _cameraFocuser.FocusOnRightNavalFactory();
                    break;

                case CameraFocuserTarget.MidLeft:
                    _cameraFocuser.FocusMidLeft();
                    break;

                case CameraFocuserTarget.Overview:
                    _cameraFocuser.FocusOnOverview();
                    break;

                default:
                    throw new ArgumentException();
            }

            OnCompleted();
        }
    }
}