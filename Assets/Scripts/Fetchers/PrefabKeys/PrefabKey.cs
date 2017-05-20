using BattleCruisers.Buildables.Buildings;
using BattleCruisers.Buildables.Units;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BattleCruisers.Fetchers.PrefabKeys
{
	public interface IPrefabKey
	{
		string PrefabPath { get; }
	}

	public abstract class PrefabKey : IPrefabKey
	{
		private string _prefabName;

		private const string PREFABS_BASE_PATH = "Prefabs/";
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

		public PrefabKey(string prefabName)
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
	}
}
