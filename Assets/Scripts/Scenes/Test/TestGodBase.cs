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
            IList<GameObject> gameObjects = GetGameObjects();
            SetActiveness(gameObjects, false);

            // Async initialisation
            UpdaterProvider updaterProvider = GetComponentInChildren<UpdaterProvider>();
            Assert.IsNotNull(updaterProvider);
            updaterProvider.Initialise();
            _updaterProvider = updaterProvider;

            Helper helper = await CreateHelperAsync(updaterProvider);

            // Child class initialisation
            Setup(helper);

            // Activate all game objects.  Everything should be initialised now, so update loops should work.
            SetActiveness(gameObjects, true);
        }

        private void SetActiveness(IList<GameObject> gameObjects, bool isActive)
        {
            foreach (GameObject gameObject in gameObjects)
            {
                gameObject.SetActive(isActive);
            }
        }

        protected virtual IList<GameObject> GetGameObjects()
        {
            return new List<GameObject>();
        }
        
        protected virtual async Task<Helper> CreateHelperAsync(IUpdaterProvider updaterProvider)
        {
            return await HelperFactory.CreateHelperAsync(updaterProvider: updaterProvider);
        }

        protected virtual void Setup(Helper helper) { }
    }
}