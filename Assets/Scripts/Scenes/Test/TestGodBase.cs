using BattleCruisers.Utils.BattleScene.Update;
using UnityEngine;
using UnityEngine.Assertions;

namespace BattleCruisers.Scenes.Test
{
    public class TestGodBase : MonoBehaviour
    {
        protected IUpdaterProvider _updaterProvider;

        protected virtual void Start()
        {
            UpdaterProvider updaterProvider = GetComponentInChildren<UpdaterProvider>();
            Assert.IsNotNull(updaterProvider);
            updaterProvider.Initialise();
            _updaterProvider = updaterProvider;
        }
    }
}