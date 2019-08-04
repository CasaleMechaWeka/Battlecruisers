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

        public CameraFocuserStep(ITutorialStepArgs args, ICameraFocuser cameraFocuser, CameraFocuserTarget target)
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
                    _cameraFocuser.FocusOnPlayerCruiser();
                    break;

                case CameraFocuserTarget.AICruiser:
                    _cameraFocuser.FocusOnAICruiser();
                    break;

                case CameraFocuserTarget.AICruiserNavalFactory:
                    _cameraFocuser.FocusOnAINavalFactory();
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