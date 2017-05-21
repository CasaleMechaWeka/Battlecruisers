using BattleCruisers.Data.Serialization;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BattleCruisers.Data
{
	public static class ApplicationModel
	{
		public static int SelectedLevel { get; set; }

		private static IDataProvider _dataProvider;
		public static IDataProvider DataProvider
		{
			get
			{
				if (_dataProvider == null)
				{
					_dataProvider = new DataProvider(
						new StaticData(),
						new Serializer(new ModelFilePathProvider()));
				}
				return _dataProvider;
			}
		}
	}
}
