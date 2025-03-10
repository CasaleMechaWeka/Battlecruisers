using System;

namespace BattleCruisers.Data.Models.PrefabKeys
{
    [Serializable]
	public class HullKey : PrefabKey
	{
		private const string HULLS_FOLDER_NAME = "Hulls";

		protected override string PrefabPathPrefix
		{
			get
			{
				return base.PrefabPathPrefix + HULLS_FOLDER_NAME + PATH_SEPARATOR;
			}
		}

		public HullKey(string prefabFileName)
			: base(prefabFileName) { }
	}
}
