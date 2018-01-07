using System;
using UnityEngine;

namespace BattleCruisers.Utils.Fetchers
{
    public class MaterialFetcher : IMaterialFetcher
    {
        private const string MATERIAL_ROOT_PATH = "Materials/";

        public Material GetMaterial(string materialName)
        {
            Material material = Resources.Load<Material>(MATERIAL_ROOT_PATH + materialName);

            if (material == null)
            {
                throw new ArgumentException("Invalid material name: " + materialName);
            }

            return material;
        }
    }
}
