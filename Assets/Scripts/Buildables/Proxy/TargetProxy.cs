using UnityEngine;
using UnityEngine.Assertions;

namespace BattleCruisers.Buildables.Proxy
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