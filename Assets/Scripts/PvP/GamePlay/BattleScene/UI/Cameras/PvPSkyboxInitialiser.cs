using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Data;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Utils;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Utils.Fetchers;
using BattleCruisers.Utils.Fetchers;
using System.Threading.Tasks;
using UnityEngine;

namespace BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.UI.Cameras
{
    public class PvPSkyboxInitialiser : MonoBehaviour
    {
        public async Task InitialiseAsync(Skybox skybox, IPvPLevel level)
        {
            PvPHelper.AssertIsNotNull(skybox, level);

            IMaterialFetcher materialFetcher = new MaterialFetcher();
            skybox.material = await materialFetcher.GetMaterialAsync(level.SkyMaterialName);
        }
    }
}