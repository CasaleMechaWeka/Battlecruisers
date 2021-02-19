using System;
using UnityEngine;

namespace BattleCruisers.Data.Models.PrefabKeys
{
	[Serializable]
	public abstract class PrefabKey : IPrefabKey
	{
		[SerializeField]
		private string _prefabName;
		public string PrefabName => _prefabName;

		private const string PREFABS_BASE_PATH = "Prefabs/BattleScene/";
		protected const char PATH_SEPARATOR = '/';

		protected virtual string PrefabPathPrefix
		{
			get
			{
				return PREFABS_BASE_PATH;
			}
		}

		public string PrefabPath
		{
			get
			{
				return PrefabPathPrefix + _prefabName;
			}
		}

        protected PrefabKey(string prefabName)
		{
			_prefabName = prefabName;
		}

		public override bool Equals(object obj)
		{
			PrefabKey other = obj as PrefabKey;
			return other != null
				&& other.PrefabPath == PrefabPath;
		}

		public override int GetHashCode()
		{
			return PrefabPath.GetHashCode();
		}

        public override string ToString()
        {
            return base.ToString() + " " + _prefabName;
        }
    }
}
