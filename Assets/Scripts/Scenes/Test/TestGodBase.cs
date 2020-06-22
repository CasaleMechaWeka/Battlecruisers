using BattleCruisers.Scenes.Test.Utilities;
using BattleCruisers.Utils.BattleScene.Update;
using BattleCruisers.Utils.Threading;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.Assertions;
using BCUtils = BattleCruisers.Utils;

namespace BattleCruisers.Scenes.Test
{
    public class TestGodBase : MonoBehaviour
    {
        protected IUpdaterProvider _updaterProvider;

        public float buildSpeedMultiplier = BCUtils.BuildSpeedMultipliers.VERY_FAST;

        private async void Start()
        {
            // Exceptions are lost in async code.  Turning exceptions off means we get a nice log message :)
            Assert.raiseExceptions = false;

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
            await SetupAsync(helper);

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

        protected virtual List<GameObject> GetGameObjects()
        {
            return new List<GameObject>();
        }
        
        protected virtual async Task<Helper> CreateHelperAsync(IUpdaterProvider updaterProvider)
        {
            // May both be null
            IDeferrer deferrer = GetComponent<TimeScaleDeferrer>();
            IDeferrer realTimeDeferrer = GetComponent<RealTimeDeferrer>();

            return 
                await HelperFactory.CreateHelperAsync(
                    updaterProvider: updaterProvider, 
                    deferrer: deferrer,
                    buildSpeedMultiplier: buildSpeedMultiplier);
        }

        protected virtual void Setup(Helper helper) { }
        protected virtual Task SetupAsync(Helper hepler) => Task.CompletedTask;
    }
}