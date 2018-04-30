using BattleCruisers.Buildables;
using UnityEngine;
using UnityEngine.Assertions;

namespace BattleCruisers.Tutorial.Steps.Providers
{
    public class SingleTargetProvider : IProvider<ITarget>
    {
        private readonly string _targetTag;

        public SingleTargetProvider(string targetTag)
        {
            _targetTag = targetTag;
        }

        public ITarget FindItem()
        {
            GameObject[] targetGameObjects = GameObject.FindGameObjectsWithTag(_targetTag);
            Assert.IsTrue(targetGameObjects.Length == 1, "Assumes there will be exactly one game object with the tag: " + _targetTag);

            ITarget target = targetGameObjects[0].GetComponentInChildren<ITarget>();
            Assert.IsNotNull(target);

            return target;
        }
    }
}
