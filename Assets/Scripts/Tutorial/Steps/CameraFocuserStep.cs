using System;
using BattleCruisers.UI.BattleScene.Navigation;
using UnityEngine.Assertions;

namespace BattleCruisers.Tutorial.Steps
{
    public enum CameraFocuserTarget
    {
        PlayerCruiser, AICruiser
    }

    // FELIX  Test :D
    public class CameraFocuserStep : TutorialStepNEW
    {
        private readonly ICameraFocuser _cameraFocuser;
        private readonly CameraFocuserTarget _target;

        public CameraFocuserStep(ITutorialStepArgsNEW args, ICameraFocuser cameraFocuser, CameraFocuserTarget target)
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
                    _cameraFocuser.FocusOnAiCruiser();
                    break;

                default:
                    throw new ArgumentException();
            }

            OnCompleted();
        }
    }
}