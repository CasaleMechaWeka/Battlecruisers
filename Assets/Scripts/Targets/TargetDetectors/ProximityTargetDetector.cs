using BattleCruisers.Cruisers.Construction;
using System;
using UnityCommon.PlatformAbstractions;

namespace BattleCruisers.Targets.TargetDetectors
{
    // FELIX  Test
    // FELIX  Use
    public class ProximityTargetDetector : ITargetDetector, IManualDetector
    {
        private readonly ITransform _parentTransform;
        private readonly IUnitProvider _potentialTargets;
        private readonly float _detectionRange;

        public event EventHandler<TargetEventArgs> TargetEntered;
        public event EventHandler<TargetEventArgs> TargetExited;

        public void Detect()
        {
            throw new NotImplementedException();
        }

        public void StartDetecting()
        {
            // empty
        }
    }
}