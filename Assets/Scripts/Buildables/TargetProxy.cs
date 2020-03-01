using UnityEngine;
using UnityEngine.Assertions;

namespace BattleCruisers.Buildables
{
    public class TargetProxy : MonoBehaviour, ITargetProxy
    {
        public ITarget Target { get; private set; }

        public void Initialise(ITarget target)
        {
            Assert.IsNotNull(target);
            Target = target;
        }
    }
}