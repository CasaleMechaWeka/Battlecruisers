using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using UnityEngine;
using System;

namespace BattleCruisers.Data.Models.PrefabKeys
{
    [Serializable]
    public class CaptainExoKey : PrefabKey
    {
		private const string HULLS_FOLDER_NAME = "CaptainExos";

		protected override string PrefabPathPrefix
		{
			get
			{
				return base.PrefabPathPrefix + HULLS_FOLDER_NAME + PATH_SEPARATOR;
			}
		}

		public CaptainExoKey(string prefabFileName)
			: base(prefabFileName) { }
    }
}
