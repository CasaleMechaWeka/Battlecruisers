using BattleCruisers.Data.Static;
using BattleCruisers.Utils.DataStrctures;
using BattleCruisers.Utils.Fetchers;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.UI;

namespace BattleCruisers.Scenes.Test.Effects.Clouds
{
    public class SkyChanger : MonoBehaviour
    {
        private Skybox _skybox;
        private ICircularList<Material> _skies;

        public Text skyName;

        async void Start()
        {
            Assert.IsNotNull(skyName);

            _skies = await FindSkyMaterialsAsync();

            _skybox = FindObjectOfType<Skybox>();
            Assert.IsNotNull(_skybox);
            ChangeSky();
        }

        private async Task<ICircularList<Material>> FindSkyMaterialsAsync()
        {
            IList<string> skyNames = new List<string>()
            {
                SkyMaterials.Midday,
                SkyMaterials.Cold,
                SkyMaterials.Sunrise,
                SkyMaterials.Purple,
                SkyMaterials.Dusk,
                SkyMaterials.Morning,
                SkyMaterials.Midnight
            };

            IList<Material> skyMaterials = new List<Material>();
            IMaterialFetcher materialFetcher = new MaterialFetcher();

            foreach (string skyName in skyNames)
            {
                skyMaterials.Add(await materialFetcher.GetMaterialAsync(skyName));
            }

            return new CircularList<Material>(skyMaterials);
        }

        public void ChangeSky()
        {
            _skybox.material = _skies.Next();
            skyName.text = _skybox.material.name;
        }
    }
}