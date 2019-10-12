using BattleCruisers.Data;
using BattleCruisers.Utils;
using BattleCruisers.Utils.Fetchers;
using System.Threading.Tasks;
using UnityEngine;

namespace BattleCruisers.UI.Cameras
{
    public class SkyboxInitialiser : MonoBehaviour
    {
        public async Task InitialiseAsync(Skybox skybox, ILevel level)
        {
            Helper.AssertIsNotNull(skybox, level);

            IMaterialFetcher materialFetcher = new MaterialFetcher();
            skybox.material = await materialFetcher.GetMaterialAsync(level.SkyMaterialName);
        }
    }
}