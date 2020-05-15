using BattleCruisers.Buildables;
using BattleCruisers.Tutorial.Highlighting;
using BattleCruisers.Tutorial.Providers;
using BattleCruisers.UI;
using UnityEngine;
using UnityEngine.Assertions;

namespace BattleCruisers.Tutorial.Steps.Providers
{
    public class SingleBuildableProvider : ISingleBuildableProvider
    {
        private readonly string _buildableTag;

        public SingleBuildableProvider(string targetTag)
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

        IHighlightable IItemProvider<IHighlightable>.FindItem()
        {
            return FindItem();
        }

        IClickableEmitter IItemProvider<IClickableEmitter>.FindItem()
        {
            return FindItem();
        }
    }
}
