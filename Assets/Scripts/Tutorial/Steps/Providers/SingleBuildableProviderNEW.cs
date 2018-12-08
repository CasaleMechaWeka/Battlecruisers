using BattleCruisers.Buildables;
using BattleCruisers.Tutorial.Highlighting.Masked;
using BattleCruisers.Tutorial.Providers;
using UnityEngine;
using UnityEngine.Assertions;

namespace BattleCruisers.Tutorial.Steps.Providers
{
    public class SingleBuildableProviderNEW : ISingleBuildableProviderNEW
    {
        private readonly string _buildableTag;

        public SingleBuildableProviderNEW(string targetTag)
        {
            _buildableTag = targetTag;
        }

        public IBuildable FindItem()
        {
            GameObject[] buildableGameObjects = GameObject.FindGameObjectsWithTag(_buildableTag);
            Assert.IsTrue(buildableGameObjects.Length == 1, "Assumes there will be exactly one game object with the tag: " + _buildableTag);

            IBuildable buildable = buildableGameObjects[0].GetComponentInChildren<IBuildable>();
            Assert.IsNotNull(buildable);

            return buildable;
        }

        IMaskHighlightable IItemProvider<IMaskHighlightable>.FindItem()
        {
            return FindItem();
        }

        IClickableEmitter IItemProvider<IClickableEmitter>.FindItem()
        {
            return FindItem();
        }
    }
}
