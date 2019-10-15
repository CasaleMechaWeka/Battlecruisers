using BattleCruisers.Scenes.Test.Utilities;
using BattleCruisers.Utils.BattleScene.Update;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.Assertions;

namespace BattleCruisers.Scenes.Test
{
    public class TestGodBase : MonoBehaviour
    {
        protected IUpdaterProvider _updaterProvider;

        // FELIX  Don't make virtual :)  Should use Setup() instead :)
        protected virtual async void Start()
        {
            // Deactivate all game objects (to avoid update loop while we are initialising)
            IList<MonoBehaviour> gameObjects = GetGameObjects();
            Helper.SetActiveness(gameObjects, false);

            // Async initialisation
            UpdaterProvider updaterProvider = GetComponentInChildren<UpdaterProvider>();
            Assert.IsNotNull(updaterProvider);
            updaterProvider.Initialise();
            _updaterProvider = updaterProvider;

            Helper helper = await CreateHelper(updaterProvider);

            // Child class initialisation
            Setup(helper);

            // Activate all game objects.  Everything should be initialised now, so update loops should work.
            Helper.SetActiveness(gameObjects, true);
        }

        protected virtual void Setup(Helper helper) { }

        protected virtual IList<MonoBehaviour> GetGameObjects()
        {
            return new List<MonoBehaviour>();
        }

        protected virtual async Task<Helper> CreateHelper(IUpdaterProvider updaterProvider)
        {
            return await HelperFactory.CreateHelperAsync(updaterProvider: updaterProvider);
        }
    }
}