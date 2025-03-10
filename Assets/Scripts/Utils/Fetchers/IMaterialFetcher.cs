using System.Threading.Tasks;
using UnityEngine;

namespace BattleCruisers.Utils.Fetchers
{
    public interface IMaterialFetcher
    {
        Task<Material> GetMaterialAsync(string materialName);
    }
}
